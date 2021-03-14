import React, { useContext, useEffect, useState } from "react";
import { DataContext } from "../../../contexts/DataContext";
import { FormHandlerContext } from "../../../contexts/FormHandlerContext";

const ProductsTable = () => {
  const { products, removeItem, currPage, changePage } = useContext(
    DataContext
  );
  const { onEditProduct } = useContext(FormHandlerContext);
  const [pageItems, setPageItems] = useState([]);
  const [findString, setFindString] = useState("");

  useEffect(() => {
    let finish = currPage * 4;
    let start = currPage - 1;
    if (products.length !== 0) {
      if (!!findString) {
        setPageItems(
          products
            .filter((prod) =>
              prod.name.toLowerCase().includes(findString.toLowerCase())
            )
            .slice(start * 4, finish)
        );
        if (currPage !== 1) {
          changePage(1);
        }
      } else {
        setPageItems(products.slice(start * 4, finish));
      }
    }
  }, [products, currPage, findString, changePage]);

  useEffect(() => {
    setFindString("");
  }, [products]);

  const findItems = (event) => {
    let name = event.target.value;
    setFindString(name);
  };

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
