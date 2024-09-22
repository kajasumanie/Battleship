import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { initializeGame, resetGame } from './redux/gameSlice';
import Grid from './components/Grid';
import './App.css';

const App = () => {
  const dispatch = useDispatch();
  const { message, status } = useSelector((state) => state.game);

  useEffect(() => {
    dispatch(initializeGame()); // Initialize the game when the app loads
  }, [dispatch]);

  const handleResetGame = () => {
    dispatch(resetGame()); // Dispatch the reset action
  };

  return (
    <div className="game-container">
      <h1>Battleship Game</h1>
      <p className="message">{message}</p>

      {status === 'loading' ? (
        <p>Loading...</p>
      ) : (
        <Grid /> // Use the Grid component to render the grid
      )}

      <button className="reset-button" onClick={handleResetGame}>
        Reset Game
      </button>
    </div>
  );
};

export default App;
