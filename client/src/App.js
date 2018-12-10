import React, { Component } from "react";
import "./App.css";
import { Route, withRouter } from "react-router-dom";
import ViewAll from "./ViewAll";
import CreateEdit from "./CreateEdit";
import LoginComponent from "./LoginComponent";
import Store from "./Store";
import { LoggedInRoute, LoggedOutRoute } from "./routes";
import { BrowserRouter } from "react-router-dom";
import { Provider } from "react-redux";
import Nav from "./Nav";

class App extends Component {
  state = {
    new: false,
    home: false
  };

  // new = () => {
  //   this.props.history.push("/create");
  // };
  // home = () => {
  //   this.history.push("/home");
  // };
  render() {
    return (
      <Provider store={Store}>
        {/* <Provider store={Store}> */}
        <BrowserRouter>
          <React.Fragment>
            <Nav />
            <LoggedInRoute exact path="/home" component={ViewAll} />
            <LoggedOutRoute path="/login" component={LoginComponent} />
            <LoggedOutRoute path="/" component={ViewAll} />
            <Route path="/register" component={Registration} />
            <Route exact path="/create" component={CreateEdit} />
            <Route exact path="/home" component={ViewAll} />
            <Route exact path="/:id(\d+)" component={CreateEdit} />
            <RegistrationComplete />
            <NavAnon>
              <IfLoggedIn>
                <Nav />
              </IfLoggedIn>
            </NavAnon>
          </React.Fragment>
        </BrowserRouter>
      </Provider>
    );
  }
}

export default withRouter(App);
