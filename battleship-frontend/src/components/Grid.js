import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { shoot } from '../redux/gameSlice';
import './Grid.css'; // You can add custom styles

const Grid = () => {
  const dispatch = useDispatch();
  const { grid, gameId } = useSelector((state) => state.game);

  const handleShoot = (x, y) => {
    if (!gameId) return;
    dispatch(shoot({ gameId, x, y }));
  };

  return (
    <table className="grid-table">
      <tbody>
        {grid.map((row, rowIndex) => (
          <tr key={rowIndex}>
            {row.split('').map((cell, cellIndex) => (
              <td
                key={cellIndex}
                onClick={() => handleShoot(rowIndex, cellIndex)}
                className={`grid-cell ${cell === 'X' ? 'hit' : cell === 'M' ? 'miss' : 'water'}`}
              >
                {cell === 'X' ? '💥' : cell === 'M' ? '🌊' : ''}
              </td>
            ))}
          </tr>
        ))}
      </tbody>
    </table>
  );
};

export default Grid;
