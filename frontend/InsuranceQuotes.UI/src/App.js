import React, { useState, useEffect } from 'react';
import axios from 'axios';
import QuoteCard from './components/QuoteCard';

function App() {
  const [quotes, setQuotes] = useState([]);
  const [searchId, setSearchId] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Fetch all quotes when component mounts
    fetchAllQuotes();
  }, []);

  const fetchAllQuotes = async () => {
    try {
      setLoading(true);
      setError('');
      const response = await axios.get('http://localhost:5228/api/quotes');
      setQuotes(response.data);
    } catch (err) {
      setError('Failed to fetch quotes. Please try again later.');
      console.error('Error fetching quotes:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleSearch = async () => {
    if (!searchId) {
      fetchAllQuotes();
      return;
    }

    try {
      setLoading(true);
      setError('');
      const response = await axios.get(`http://localhost:5228/api/quotes/${searchId}`);
      setQuotes([response.data]);
    } catch (err) {
      if (err.response?.status === 404) {
        setError(`No quote found with ID: ${searchId}`);
        setQuotes([]);
      } else {
        setError('Failed to fetch quote. Please try again later.');
      }
      console.error('Error fetching quote:', err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container">
      <h1>Insurance Quotes</h1>
      
      <div className="search-container">
        <input
          type="number"
          className="search-input"
          placeholder="Enter quote ID..."
          value={searchId}
          onChange={(e) => setSearchId(e.target.value)}
          min="1"
        />
        <button className="search-button" onClick={handleSearch}>
          Search
        </button>
        {searchId && (
          <button className="search-button" onClick={() => {
            setSearchId('');
            fetchAllQuotes();
          }}>
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
    </div>
  );
}

export default App;