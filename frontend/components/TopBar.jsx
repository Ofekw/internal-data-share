import React from 'react';
import AppBar from 'material-ui/AppBar';
import IconButton from 'material-ui/IconButton';
import FlatButton from 'material-ui/FlatButton';

 const TopBar = () => (
  <AppBar
    title={<span>Title</span>}
    iconElementRight={<FlatButton label="Save" />}
  />
);

export default TopBar;