import axios from 'axios';

const API_URL = 'https://localhost:5000/api/producto';

export const obtenerProductos = async () => {
  const respuesta = await axios.get(API_URL);
  return respuesta.data;
};
