import React, { Component } from "react";
import { PaginationContext } from "../../../contexts/PaginationContext";
import { DataContext } from "../../../contexts/DataContext";

class CategoriesTable extends Component {
  static contextType = PaginationContext;
  onRemove = async (id) => {
    const { removeItem, pageHelper, pageConstr, currPage } = this.context;

    await removeItem(id);
    pageHelper();
    const { totalPages } = this.context;
    let tpLength = totalPages.length;
    console.log(tpLength);
    if (tpLength < currPage) {
      pageConstr(tpLength);
    } else {
      pageConstr(currPage);
    }
  };
  render() {
    return (
      <DataContext.Consumer>
        {(DataContext) => (
          <PaginationContext.Consumer>
            {(paginationContext) => {
              const { editCategory } = DataContext;
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
                      <th>Photo</th>
                      <th>Action</th>
                    </tr>
                  </thead>
                  <tbody>
                    {pageItems.map((category) => (
                      <tr key={category.categoryId}>
                        <td style={{ textAlign: "left" }}>{category.name}</td>
                        <td>
                          <img
                            src={`WebImage/GetImage/${category.photo}`}
                            style={{
                              width: 50 + "px",
                              height: 50 + "px",
                              objectFit: "cover",
                            }}
                            alt="category"
                          />
                        </td>
                        <td>
                          <button
                            className="btn btn-outline-warning btn-sm"
                            style={{ marginRight: 10 + "px" }}
                            onClick={() => editCategory(category)}
                          >
                            Edit
                          </button>
                          <button
                            className="btn btn-outline-danger btn-sm"
                            onClick={() => this.onRemove(category.categoryId)}
                          >
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

export default CategoriesTable;
