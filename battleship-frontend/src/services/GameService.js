import axios from 'axios';

const BASE_URL = 'https://localhost:7246/api/Battleship'; // Base API URL

const GameService = {
  initializeGame: async () => {
    const response = await axios.post(`${BASE_URL}/initialize`);
    return response.data;
  },

  getStatus: async (gameId) => {
    const response = await axios.get(`${BASE_URL}/status/${gameId}`);
    return response.data;
  },

  shoot: async (gameId, x, y) => {
    const response = await axios.post(`${BASE_URL}/shoot/${gameId}`, { x, y });
    return response.data;
  },

  resetGame: async () => {
    await axios.post(`${BASE_URL}/reset`);
  }
};

export default GameService;
