import React from 'react';
import ReactDOM from 'react-dom';
import IconMenu from 'material-ui/IconMenu';
import {List, ListItem} from 'material-ui/List'
import Popover from 'material-ui/Popover';
import Menu from 'material-ui/Menu';
import MenuItem from 'material-ui/MenuItem';
import IconButton from 'material-ui/IconButton/IconButton';
import Edit from 'material-ui/svg-icons/editor/mode-edit';
import Delete from 'material-ui/svg-icons/action/Delete';
import MoreVertIcon from 'material-ui/svg-icons/navigation/more-vert';
import {Card, CardActions, CardHeader, CardText} from 'material-ui/Card';
import FlatButton from 'material-ui/FlatButton';
import ContentCopy from 'material-ui/svg-icons/content/content-copy';
import EditModeField from './EditModeField.jsx';
import TextField from 'material-ui/TextField';
import RaisedButton from 'material-ui/RaisedButton';
import Save from 'material-ui/svg-icons/content/save';

const styles = {
  paleGrey: {
    'background-color': '#ddd',
  },
  keyField: {
    width: '100px',
    marginRight: '10px',
  },
  rightAllign: {
    float: 'right',
  }
};


class CardExampleExpandable extends React.Component {
  constructor(props) {
    super(props);

    this.key = props.identifier;
    this.value = props.value;

    this.state = {
      open: false,
      editable: props.editable
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

  enableEditMode = () => {
    this.setState({
      editable: true
    });
  }

  disableEditMode = () => {
    this.setState({
      editable: false
    });
  }

  handleKeyChange = (event) => {
    this.key = event.target.value;
  }

  handleValueChange = (event) => {
    this.value = event.target.value;
  }

  render() {
    if (this.state.editable) {
      return (
        <ListItem>
          <TextField
            name="key"
            floatingLabelText="Key"
            defaultValue={this.key}
            style={styles.keyField}
            onChange={this.handleKeyChange}
          />
          <TextField
            name="value"
            floatingLabelText="Value"
            defaultValue={this.value}
            style={styles.valueField}
            onChange={this.handleValueChange}
          />
          <RaisedButton
            icon={<Save />}
            style={styles.rightAllign}
            onTouchTap={this.disableEditMode}
          />
        </ListItem>
      );
    } else {
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
              <MenuItem primaryText="Edit" leftIcon={<Edit />} onTouchTap={this.enableEditMode}/>
              <MenuItem primaryText="Delete" leftIcon={<Delete />}/>
            </Menu>
          </Popover>
        </ListItem>
      );
    }
  }
}
export default CardExampleExpandable;
