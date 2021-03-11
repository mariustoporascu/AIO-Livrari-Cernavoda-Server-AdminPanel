import React, { Component } from "react";
import { PaginationContext } from "../contexts/PaginationContext";

class Pagination extends Component {
  render() {
    return (
      <PaginationContext.Consumer>
        {(context) => {
          const { totalPages, currPage, pageConstr } = context;
          return (
            <div style={{ display: "flex", justifyContent: "center" }}>
              {totalPages.map((page) => (
                <button
                  key={page}
                  className={
                    page === currPage
                      ? "active btn btn-outline-info btn-sm"
                      : "btn btn-outline-info btn-sm"
                  }
                  style={{
                    marginRight: 10 + "px",
                  }}
                  onClick={() => pageConstr(page)}
                >
                  Page {page}
                </button>
              ))}
            </div>
          );
        }}
      </PaginationContext.Consumer>
    );
  }
}

export default Pagination;
