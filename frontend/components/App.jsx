import React from 'react';
import RaisedButton from 'material-ui/RaisedButton';

var alertTest = function () {
  alert('hello');
};

class AlertButton extends React.Component {
  handleTouchTap() {
    alert('hello');
  }

  render() {
    return <RaisedButton label="Default" onTouchTap={this.handleTouchTap} />;
  }
}

export default AlertButton;
