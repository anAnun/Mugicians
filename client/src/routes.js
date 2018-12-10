import React from "react";
import { Route, Redirect, withRouter } from "react-router-dom";
import { connect } from "react-redux";

const REDIRECT_WHEN_LOGGED_OUT = "/";
const REDIRECT_WHEN_LOGGED_IN = "/home";

function LoggedOut({ currentUser, component, render, ...rest }) {
  return (
    <Route
      {...rest}
      render={props =>
        currentUser === null ? null : currentUser === false ? (
          component ? (
            <Route component={component} {...props} />
          ) : (
            <Route render={render} {...props} />
          )
        ) : (
          <Redirect to={REDIRECT_WHEN_LOGGED_IN} />
        )
      }
    />
  );
}

function LoggedIn({ currentUser, component, render, ...rest }) {
  return (
    <Route
      {...rest}
      render={props =>
        currentUser === null ? null : currentUser === false ? (
          <Redirect
            to={
              REDIRECT_WHEN_LOGGED_OUT +
              "?returnurl=" +
              encodeURIComponent(props.location.pathname)
            }
          />
        ) : component ? (
          <Route component={component} {...props} />
        ) : (
          <Route render={render} {...props} />
        )
      }
    />
  );
}

function mapStateToProps(state) {
  return {
    currentUser: state.currentUser
  };
}

export const LoggedOutRoute = withRouter(connect(mapStateToProps)(LoggedOut));
export const LoggedInRoute = withRouter(connect(mapStateToProps)(LoggedIn));
