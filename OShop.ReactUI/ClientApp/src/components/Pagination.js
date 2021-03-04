/*eslint unicode-bom: ["error", "always"]*/
import React, { Component } from "react";

export default class Pagination extends Component {
  static pagination(pageNmbr, itemsArray, itemsPerPage) {
    var currentPageIndex = pageNmbr * itemsPerPage;
    var totalItems = itemsArray.length;
    var i;
    var filter = [];
    if (currentPageIndex > totalItems)
      for (i = currentPageIndex - itemsPerPage; i < totalItems; i++) {
        filter.push(itemsArray[i]);
      }
    else {
      for (i = currentPageIndex - itemsPerPage; i < currentPageIndex; i++) {
        filter.push(itemsArray[i]);
      }
    }
    return {
      filter,
      pageNmbr,
    };
  }

  static pageHelper(itemsArray, itemsPerPage) {
    var totalItems = itemsArray.length;
    var totPages = Math.ceil(totalItems / itemsPerPage);
    var i;
    var pages = [];
    for (i = 1; i < totPages + 1; i++) {
      pages.push(i);
    }
    return pages;
  }

  render() {
    return (
      <div style={{ display: "flex", justifyContent: "center" }}>
        {this.props.counterStates.totalPages.map((page) => (
          <button
            key={page}
            className={
              page === this.props.counterStates.currPage
                ? "active btn btn-outline-warning btn-sm"
                : "btn btn-outline-warning btn-sm"
            }
            style={{ marginRight: 10 + "px" }}
            onClick={() => this.props.passChangePage(page)}
          >
            Page {page}
          </button>
        ))}
      </div>
    );
  }
}
