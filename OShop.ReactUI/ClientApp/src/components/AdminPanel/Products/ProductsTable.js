import React, { useContext } from "react";
import { DataContext } from "../../../contexts/DataContext";
import { FormHandlerContext } from "../../../contexts/FormHandlerContext";
import { PaginationContext } from "../../../contexts/PaginationContext";

const ProductsTable = () => {
  const { pageItems } = useContext(PaginationContext);
  const { findItems, removeItem } = useContext(DataContext);
  const { onEditProduct } = useContext(FormHandlerContext);

  return (
    <div>
      <input
        className="text-center"
        type="text"
        placeholder="Search product"
        onChange={findItems}
      />
      <table
        className="table table-striped"
        aria-labelledby="tabelLabel"
        style={{
          textAlign: "center",
          border: 2 + "px solid black",
        }}
      >
        <thead>
          <tr>
            <th style={{ textAlign: "left" }}>Name</th>
            <th>Stock</th>
            <th>Price</th>
            <th>Photo</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {pageItems.map((product) => (
            <tr key={product.productId}>
              <td style={{ textAlign: "left" }}>{product.name}</td>
              <td>{product.stock}</td>
              <td>{product.price}</td>
              <td>
                <img
                  src={`WebImage/GetImage/${product.photo}`}
                  style={{
                    width: 50 + "px",
                    height: 50 + "px",
                    objectFit: "cover",
                  }}
                  alt="product"
                />
              </td>
              <td>
                <button
                  className="btn btn-outline-warning btn-sm"
                  style={{ marginRight: 10 + "px" }}
                  onClick={() => onEditProduct(product)}
                >
                  Edit
                </button>
                <button
                  className="btn btn-outline-danger btn-sm"
                  onClick={() => removeItem(product.productId)}
                >
                  Remove
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ProductsTable;
