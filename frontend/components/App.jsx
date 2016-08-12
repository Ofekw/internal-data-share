import React from 'react';
import RaisedButton from 'material-ui/RaisedButton';
import TopBar from './TopBar.jsx';

var alertTest = function () {
  alert('hello');
};

class AlertButton extends React.Component {
  handleTouchTap() {
    alert('hello');
  }

  render() {
    return <div>
      <TopBar/>
      </div>;
  }
}

export default AlertButton;
