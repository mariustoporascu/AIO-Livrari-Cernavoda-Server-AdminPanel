/*eslint unicode-bom: ["error", "always"]*/
import React, { Component } from "react";

export default class Loading extends Component {
  render() {
    return (
      <div
        className="text-center"
        style={{
          position: "absolute",
          top: 50 + "%",
          left: 47 + "%",
          margin: "auto",
        }}
      >
        <div
          className="spinner-border"
          style={{ width: 5 + "em", height: 5 + "em" }}
          role="status"
        >
          <span className="sr-only">Loading...</span>
        </div>
      </div>
    );
  }
}
