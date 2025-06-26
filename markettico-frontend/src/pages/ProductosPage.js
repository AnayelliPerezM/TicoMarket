import React, { useEffect, useState } from 'react';
import { obtenerProductos } from '../services/productoService';

const ProductosPage = () => {
    const [productos, setProductos] = useState([]);

    useEffect(() => {
        obtenerProductos()
            .then(data => setProductos(data))
            .catch(err => console.error('Error al cargar productos:', err));
    }, []);

    return (
        <div className="container mt-4">
            <h2 className="mb-4">Productos disponibles</h2>
            <div className="row">
                {productos.map(p => (
                    <div className="col-md-4 mb-4" key={p.productoId}>
                        <div className="card h-100">
                            <div className="card-body">
                                <h5 className="card-title">{p.nombre}</h5>
                                <p className="card-text">{p.descripcion}</p>
                                <p><strong>Precio:</strong> ₡{p.precio}</p>
                                <p><strong>Categoría:</strong> {p.categoria}</p>
                                <p><strong>Usuario:</strong> #{p.usuarioId}</p>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default ProductosPage;
