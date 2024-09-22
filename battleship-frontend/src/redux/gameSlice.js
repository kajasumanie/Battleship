import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import GameService from '../services/GameService';

// Async Thunks for API calls using GameService
export const initializeGame = createAsyncThunk('game/initializeGame', async () => {

     // Sample grid for testing
  const sampleGrid = [
    'OOOOO',
    'OXOOO',
    'OOOOO',
    'OOOXO',
    'OOOOO'
  ];
  const gameId = await GameService.initializeGame();

  const grid = await GameService.getStatus(gameId);

  return { gameId, grid: grid }; // Return both gameId and grid
});

export const getStatus = createAsyncThunk('game/getStatus', async (gameId) => {
  const grid = await GameService.getStatus(gameId);
  return grid;
});

export const shoot = createAsyncThunk('game/shoot', async ({ gameId, x, y }) => {
  const message = await GameService.shoot(gameId, x, y);
  const grid = await GameService.getStatus(gameId);
  return { message, grid: grid }; // Return both gameId and grid

});

export const resetGame = createAsyncThunk('game/resetGame', async () => {
  await GameService.resetGame();
  const gameId = await GameService.initializeGame();

  const grid = await GameService.getStatus(gameId);
  return { gameId, grid: grid }; // Return both gameId and grid
});

const gameSlice = createSlice({
    name: 'game',
    initialState: {
      grid: [],  // Initially empty grid
      gameId: null,
      message: '',
      status: 'idle',  // 'idle' | 'loading' | 'failed'
    },
    reducers: {
      setGameId: (state, action) => {
        state.gameId = action.payload;
      },
      setMessage: (state, action) => {
        state.message = action.payload;
      },
    },
    extraReducers: (builder) => {
      builder
        .addCase(initializeGame.pending, (state) => {
          state.status = 'loading';
        })
        .addCase(initializeGame.fulfilled, (state, action) => {
          state.gameId = action.payload.gameId;
          state.grid = action.payload.grid;  // Set the grid with sample data
          state.status = 'idle';
        })
        .addCase(getStatus.fulfilled, (state, action) => {
          state.grid = action.payload;
        })
        .addCase(shoot.fulfilled, (state, action) => {
          state.message = action.payload.message;
          state.grid = action.payload.grid;

        })
        .addCase(resetGame.fulfilled, (state, action) => {
          state.grid = action.payload.grid;
          state.message = 'Game has been reset.';
          state.gameId = action.payload.gameId;
        });
    },
  });
  

export const { setGameId, setMessage } = gameSlice.actions;

export default gameSlice.reducer;
