/*eslint unicode-bom: ["error", "always"]*/
import React, { Component } from "react";

export default class ProductsTable extends Component {
  render() {
    return (
      <div>
        <table className="table table-striped" aria-labelledby="tabelLabel">
          <thead>
            <tr>
              <th>Name</th>
              <th>Category</th>
              <th>Stock</th>
              <th>Price</th>
              <th>Photo</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            {this.props.counterStates.pageItems.map((product) => (
              <tr key={product.productId}>
                <td>{product.name}</td>
                <td>
                  {
                    this.props.counterStates.categories.filter(
                      (category) =>
                        category.categoryId === product.categoryRefId
                    )[0].name
                  }
                </td>
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
                  />
                </td>
                <td>
                  <button
                    className="btn btn-outline-warning btn-sm"
                    style={{ marginRight: 10 + "px" }}
                    onClick={() => this.props.passEditProduct(product)}
                  >
                    Edit
                  </button>
                  <button
                    className="btn btn-outline-danger btn-sm"
                    onClick={() => this.props.passRemoveProduct(product.name)}
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
  }
}
