import React, { Component } from "react";
import { PaginationContext } from "../../../contexts/PaginationContext";
import { DataContext } from "../../../contexts/DataContext";

class ProductsTable extends Component {
  render() {
    return (
      <DataContext.Consumer>
        {(DataContext) => (
          <PaginationContext.Consumer>
            {(paginationContext) => {
              const { editProduct } = DataContext;
              const { pageItems } = paginationContext;
              return (
                <table
                  className="table table-striped"
                  aria-labelledby="tabelLabel"
                  style={{ textAlign: "center" }}
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
                            onClick={() => editProduct(product)}
                          >
                            Edit
                          </button>
                          <button className="btn btn-outline-danger btn-sm">
                            Remove
                          </button>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              );
            }}
          </PaginationContext.Consumer>
        )}
      </DataContext.Consumer>
    );
  }
}

export default ProductsTable;
