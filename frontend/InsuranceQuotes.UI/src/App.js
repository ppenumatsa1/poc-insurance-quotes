import React, { useState, useEffect, useCallback } from 'react';
import axios from 'axios';
import QuoteCard from './components/QuoteCard';
import { useMsal, useIsAuthenticated } from '@azure/msal-react';
import { InteractionStatus, InteractionRequiredAuthError } from '@azure/msal-browser';
import { loginRequest } from './authConfig';

function App() {
  const [quotes, setQuotes] = useState([]);
  const [searchId, setSearchId] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(true);
  
  const { instance, inProgress } = useMsal();
  const isAuthenticated = useIsAuthenticated();

  const getAccessToken = useCallback(async () => {
    if (inProgress !== InteractionStatus.None) {
      return null;
    }

    const currentAccount = instance.getActiveAccount() || instance.getAllAccounts()[0];
    
    if (!currentAccount) {
      console.log('No active account found');
      return null;
    }

    try {
      const response = await instance.acquireTokenSilent({
        ...loginRequest,
        account: currentAccount
      });
      return response.accessToken;
    } catch (error) {
      console.log('Silent token acquisition failed:', error);
      if (error instanceof InteractionRequiredAuthError) {
        try {
          const response = await instance.acquireTokenRedirect(loginRequest);
          return response?.accessToken;
        } catch (redirectError) {
          console.error('Token acquisition failed:', redirectError);
          setError('Authentication failed. Please try signing in again.');
          return null;
        }
      }
      setError('Authentication failed. Please try signing in again.');
      return null;
    }
  }, [instance, inProgress]);

  const fetchAllQuotes = useCallback(async () => {
    try {
      setLoading(true);
      setError('');
      const token = await getAccessToken();
      
      if (!token) {
        setError('Authentication failed. Please try signing in again.');
        return;
      }

      if (!process.env.REACT_APP_API_URL) {
        throw new Error('API URL is not configured. Please check .env.local file.');
      }

      const response = await axios.get(`${process.env.REACT_APP_API_URL}/quotes`, {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });
      setQuotes(response.data);
    } catch (err) {
      console.error('Detailed error:', err);
      setError('Failed to fetch quotes. Please try again later.');
    } finally {
      setLoading(false);
    }
  }, [getAccessToken]);

  useEffect(() => {
    if (isAuthenticated && inProgress === InteractionStatus.None) {
      fetchAllQuotes();
    }
  }, [isAuthenticated, inProgress, fetchAllQuotes]);

  const handleSearch = async () => {
    if (!searchId) {
      fetchAllQuotes();
      return;
    }

    try {
      setLoading(true);
      setError('');
      const token = await getAccessToken();

      if (!token) {
        setError('Authentication failed. Please try signing in again.');
        return;
      }

      if (!process.env.REACT_APP_API_URL) {
        throw new Error('API URL is not configured. Please check .env.local file.');
      }

      const response = await axios.get(`${process.env.REACT_APP_API_URL}/quotes/${searchId}`, {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });
      setQuotes([response.data]);
    } catch (err) {
      console.error('Detailed error:', err);
      if (err.response?.status === 404) {
        setError(`No quote found with ID: ${searchId}`);
        setQuotes([]);
      } else {
        setError('Failed to fetch quote. Please try again later.');
      }
    } finally {
      setLoading(false);
    }
  };

  const handleLogin = async () => {
    if (inProgress === InteractionStatus.None) {
      try {
        await instance.loginRedirect(loginRequest);
      } catch (error) {
        console.error('Login failed:', error);
        setError('Login failed. Please try again.');
      }
    }
  };

  const handleLogout = async () => {
    if (inProgress === InteractionStatus.None) {
      await instance.logoutPopup();
    }
  };

  // Select first account if no active account is set
  useEffect(() => {
    const accounts = instance.getAllAccounts();
    if (accounts.length > 0 && !instance.getActiveAccount()) {
      instance.setActiveAccount(accounts[0]);
    }
  }, [instance]);

  return (
    <div className="container">
      <div className="auth-container" style={{ textAlign: 'right', padding: '10px' }}>
        {isAuthenticated ? (
          <button 
            className="auth-button" 
            onClick={handleLogout}
            disabled={inProgress !== InteractionStatus.None}
          >
            Sign Out
          </button>
        ) : (
          <button 
            className="auth-button" 
            onClick={handleLogin}
            disabled={inProgress !== InteractionStatus.None}
          >
            Sign In
          </button>
        )}
      </div>

      <h1>Insurance Quotes</h1>
      
      {isAuthenticated ? (
        <>
          <div className="search-container">
            <input
              type="number"
              className="search-input"
              placeholder="Enter quote ID..."
              value={searchId}
              onChange={(e) => setSearchId(e.target.value)}
              min="1"
            />
            <button 
              className="search-button" 
              onClick={handleSearch}
              disabled={inProgress !== InteractionStatus.None}
            >
              Search
            </button>
            {searchId && (
              <button 
                className="search-button" 
                onClick={() => {
                  setSearchId('');
                  fetchAllQuotes();
                }}
                disabled={inProgress !== InteractionStatus.None}
              >
                Show All
              </button>
            )}
          </div>

          {error && <div className="error-message">{error}</div>}
          
          {loading ? (
            <p>Loading quotes...</p>
          ) : (
            <div className="quotes-grid">
              {quotes.map(quote => (
                <QuoteCard key={quote.id} quote={quote} />
              ))}
            </div>
          )}
        </>
      ) : (
        <div className="login-message" style={{ textAlign: 'center', marginTop: '50px' }}>
          <p>Please sign in to view insurance quotes.</p>
        </div>
      )}
    </div>
  );
}

export default App;