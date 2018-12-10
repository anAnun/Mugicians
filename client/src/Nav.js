import React from "react";
import { withRouter } from "react-router-dom";

class Nav extends React.Component {
  render() {
    return (
      <React.Fragment>
        <div className="image">
          <div className="App-header piano">
            <button
              className={"nav"}
              onClick={() => this.props.history.push("/home")}
            >
              Home
            </button>
            <button
              className={"nav"}
              onClick={() => this.props.history.push("/create")}
            >
              New Song
            </button>
          </div>
        </div>
      </React.Fragment>
    );
  }
}
export default withRouter(Nav);
