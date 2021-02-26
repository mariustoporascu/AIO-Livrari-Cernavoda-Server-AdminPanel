import React, { Component } from "react";

export default class CategoriesTable extends Component {
  render() {
    return (
      <div>
        <table className="table table-striped" aria-labelledby="tabelLabel">
          <thead>
            <tr>
              <th>Name</th>
              <th>Photo</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            {this.props.counterStates.pageItems.map(category => (
              <tr key={category.categoryId}>
                <td>{category.name}</td>
                <td>
                  <img
                    src={`WebImage/GetImage/${category.photo}`}
                    style={{
                      width: 50 + "px",
                      height: 50 + "px",
                      objectFit: "cover"
                    }}
                    alt="categoryphoto"
                  />
                </td>
                <td>
                  <button
                    className="btn btn-outline-warning btn-sm"
                    style={{ marginRight: 10 + "px" }}
                    onClick={() => this.props.passEditCategory(category)}
                  >
                    Edit
                  </button>
                  <button
                    className="btn btn-outline-danger btn-sm"
                    onClick={() => this.props.passRemoveCategory(category.name)}
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
