import React from 'react';
import IconMenu from 'material-ui/IconMenu';
import {List, ListItem} from 'material-ui/List'
import Popover from 'material-ui/Popover';
import Menu from 'material-ui/Menu';
import MenuItem from 'material-ui/MenuItem';
import IconButton from 'material-ui/IconButton/IconButton';
import MoreVertIcon from 'material-ui/svg-icons/navigation/more-vert';
import {Card, CardActions, CardHeader, CardText} from 'material-ui/Card';
import FlatButton from 'material-ui/FlatButton';
import ContentCopy from 'material-ui/svg-icons/content/content-copy';
import EditModeField from './EditModeField.jsx';

const styles = {
  paleGrey: {
    'background-color': '#ddd',
  },
};


class CardExampleExpandable extends React.Component {
  constructor(props) {
    super(props);

    this.key = props.identifier;
    this.value = props.value;

    this.state = {
      open: false,
    };
  }

  openMenu = (event) => {
    // This prevents ghost click.
    event.preventDefault();

    console.log('Opening menu.');

    this.setState({
      open: true,
      anchorEl: event.currentTarget,
    });
  };

  handleRequestClose = () => {
    this.setState({
      open: false,
    });
  };

  render() {
    return (
      <ListItem primaryText={this.value} secondaryText={this.key} rightIcon={<MoreVertIcon />} onTouchTap={this.openMenu}>
        <Popover
          open={this.state.open}
          anchorEl={this.state.anchorEl}
          anchorOrigin={{horizontal: 'left', vertical: 'bottom'}}
          targetOrigin={{horizontal: 'left', vertical: 'top'}}
          onRequestClose={this.handleRequestClose}
        >
          <Menu>
            <MenuItem primaryText="Edit" />
            <MenuItem primaryText="Delete" />
          </Menu>
        </Popover>
      </ListItem>
    );
  }
}
export default CardExampleExpandable;
