import React, { useState } from 'react';
import './SearchResults.css';

function Search() {
    const [searchTerm, setSearchTerm] = useState('');
    const [searchResults, setSearchResults] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState('');
    const [selectedMovie, setSelectedMovie] = useState(null);
    const [trailer, setTrailer] = useState(null);


    const fetchTrailer = async (title) => {
        try {
            const trailerResponse = await fetch(`https://localhost:7106/Trailer/${encodeURIComponent(title)}`, {
                method: 'GET'
            });

            console.log('Fetching trailer for:', title); 

            if (!trailerResponse.ok) {
                throw new Error(`Error: ${trailerResponse.status}`);
            }

            const trailerData = await trailerResponse.json();
            console.log('Trailer data:', trailerData); 

            setTrailer(trailerData);
        } catch (err) {
            console.error('Error fetching trailer:', err);
        }
    };

    const handleResultClick = async (movieId, title) => {
        setIsLoading(true);
        setError('');
        setTrailer(null); // Reset trailer state


        try {
            const response = await fetch(`https://localhost:7106/Movies/Get?id=${encodeURIComponent(movieId)}`, {
                method: 'GET'
            });

            if (!response.ok) {
                setError(`Error: ${response.status}`);
                setIsLoading(false);
                return;
            }

            const data = await response.json();
            setSelectedMovie(data);
            await fetchTrailer(data.title);

        } catch (err) {
            setError('Failed to fetch movie details');
            console.error('Error fetching movie details:', err);
        }

        setIsLoading(false);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setIsLoading(true);
        setError('');

        try {
            const response = await fetch(`https://localhost:7106/Movies/Search?title=${encodeURIComponent(searchTerm)}`, {
                method: 'GET'
            });

            if (!response.ok) {
                setError(`Error: ${response.status}`);
                setIsLoading(false);
                return;
            }

            const data = await response.json();
            setSearchResults(data);
        } catch (err) {
            setError('Failed to fetch movies');
            console.error('Error fetching movies:', err);
        }

        setIsLoading(false);
    };

    const handleInputChange = (e) => {
        setSearchTerm(e.target.value);
    };

    return (
        <div className="search-container">
            <h1>Search</h1>
            <form onSubmit={handleSubmit}>
                <input
                    type="text"
                    value={searchTerm}
                    onChange={handleInputChange}
                    placeholder="Enter search term"
                />
                <button type="submit" disabled={isLoading}>Search</button>
            </form>

            {isLoading && <p>Loading...</p>}
            {error && <p>Error: {error}</p>}

            <ul>
                {searchResults.map((result, index) => (
                    <li key={index} className="searchResult" onClick={() => handleResultClick(result.imdb_id)}>
                        {result.title} (IMDb ID: {result.imdb_id})
                    </li>
                ))}
            </ul>

            {selectedMovie && (
                <div className="movie-details">
                    <h2>Movie Details</h2>
                    <img src={selectedMovie.image_url} alt={selectedMovie.title} className="movie-image" />
                    <p><strong>Title:</strong> {selectedMovie.title}</p>
                    <p><strong>Description:</strong> {selectedMovie.description}</p>
                    <p><strong>Year:</strong> {selectedMovie.year}</p>
                    <p><strong>Popularity:</strong> {selectedMovie.popularity}</p>
                    <p><strong>Content Rating:</strong> {selectedMovie.content_rating}</p>
                    <p><strong>Movie Length:</strong> {selectedMovie.movie_length} minutes</p>
                    <p><strong>Rating:</strong> {selectedMovie.rating}</p>
                    <p><strong>Release Date:</strong> {new Date(selectedMovie.release).toLocaleDateString()}</p>
                    <p><strong>Plot:</strong> {selectedMovie.plot}</p>
                    <p><strong>Type:</strong> {selectedMovie.type}</p>
                    <p><strong>Genres:</strong> {selectedMovie.gen.map(genre => genre.genre).join(', ')}</p>
                    <p><strong>Trailer:</strong> <a href={selectedMovie.trailer} target="_blank" rel="noopener noreferrer">Watch Trailer</a></p>
                    <p><strong>Banner:</strong> <img src={selectedMovie.banner} alt="Banner" className="movie-banner" /></p>
                </div>
            )}
            {trailer && trailer.videoId && (
                <div className="trailer">
                    <h2>Trailer</h2>
                    <iframe
                        width="560"
                        height="315"
                        src={`https://www.youtube.com/embed/${trailer.videoId}`}
                        allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                        allowFullScreen>
                    </iframe>
                </div>
            )}
        </div>
    );
}

export default Search;
