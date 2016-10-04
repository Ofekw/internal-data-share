import React from 'react';
import IconMenu from 'material-ui/IconMenu';
import {List, ListItem} from 'material-ui/List'
import Popover from 'material-ui/Popover';
import Menu from 'material-ui/Menu';
import MenuItem from 'material-ui/MenuItem';
import IconButton from 'material-ui/IconButton/IconButton';
import TextField from 'material-ui/TextField';
import $ from 'jquery';
import Clipboard from 'clipboard-js';
import config from '../config.js';
import async from '../node_modules/async/dist/async.min.js';
import {grey400, darkBlack, lightBlack} from 'material-ui/styles/colors';
import Snackbar from 'material-ui/Snackbar';

// Icons
import Edit from 'material-ui/svg-icons/editor/mode-edit';
import Delete from 'material-ui/svg-icons/action/delete';
import MoreVertIcon from 'material-ui/svg-icons/navigation/more-vert';
import FlatButton from 'material-ui/FlatButton';
import ContentCopy from 'material-ui/svg-icons/content/content-copy';
import Save from 'material-ui/svg-icons/content/save';
import Undo from 'material-ui/svg-icons/content/undo';
import Error from 'material-ui/svg-icons/alert/error';
import ErrorOutline from 'material-ui/svg-icons/alert/error-outline';

const styles = {
  paleGrey: {
    'background-color': '#ddd',
  },
  keyField: {
    width: '20%',
    marginRight: '10px',
    display: 'inline-block',
    position: 'relative'
  },
  rightAllign: {
    float: 'right',
    display: 'inline-block',
    position: 'relative',
  }
};

const clean = 'clean';
const dirty = 'dirty';
const deleted = 'deleted';
const neww = 'new';
const newDeleted = 'newDeleted'

// Key value text field that can be toggled between view and edit modes.
class ModalField extends React.Component {
  constructor(props) {
    super(props);

    this.key = props.identifier;
    this.value = props.value;

    var isDirty = this.props.new ? neww : clean;
    var stale = this.props.stale;

    this.state = {
      dirty: isDirty,
      stale: stale,
      open: false,
      copy: false,
      editable: props.editable,
    };
  }

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

  disableEditMode = (event) => {
    event.preventDefault();

    this.setState({
      editable: false
    });
  }

  handleKeyChange = (event) => {
    this.key = event.target.value;

    this.setState({ keyValue: this.key });
    // Set state to dirty so we know to save this change.
    if (this.state.dirty !== neww) {
      this.setState({ dirty: dirty });
    }
  }

  handleValueChange = (event) => {
    this.value = event.target.value;

    this.setState({ valueValue: this.value });
    // Set state to dirty so we know to save this change.
    if (this.state.dirty !== neww) {
      this.setState({ dirty: dirty });
    }
  }

  toggleDeleted = (event) => {
    if (this.state.dirty === neww) {
      this.setState({ dirty: newDeleted });
      return;
    }
    if (this.state.dirty === newDeleted) {
      this.setState({ dirty: neww })
      return;
    }
    if (this.state.dirty !== deleted) {
      // Set state to deleted so we know to save this change.
      this.setState({ dirty: deleted });
    } else {
      // Undelete this node
      if (this.state.dirty !== neww) {
        this.setState({ dirty: clean });
      }
    }
  }

  toggleStale = (event) => {
    // Update dirty flag if needed.
    console.log(this.state.dirty);
    if (this.state.dirty === clean) {
      this.setState({dirty: dirty});
    }

    // Toggle staleness
    if (this.state.stale) {
      this.setState({
        stale: false
      });
      return;
    }
    if (!this.state.stale) {
      this.setState({
        stale: true
      });
      return;
    }
  }

  copyToClipboard = () => {
    var self = this;
    // Copies the value of this key-value to the clipboard, will fail on unsupported browsers.
    var value = this.value.split(':')[1];
    Clipboard.copy(this.value).then(
      function () {
        self.setState({
          open: true,
          copy: true
        });
        console.log("success");
      },
      function (err) {
        self.setState({
          open: true,
          copy: false
        });
        console.log("failure", err);
      }
    );
  }

  createNew = () => {
    // Does error checking
    var key = this.state.keyValue;
    var value = this.state.valueValue;
    if (!key) {
      this.setState({ keyError: "Input a key" });
    }
    if (!value) {
      this.setState({ valueError: "Input a value" });
    }
    if (!key || !value) {
      return;
    }

    this.setState({
      keyValue: '',
      valueValue: ''
    });
    this.props.createNew(key, value);
  }

  isUrl(s) {
     var regexp = /(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/
     return regexp.test(s);
  }

  render() {
    var buttonStyle = {
      display: 'inline-block',
      position: 'relative',
    };

    if (this.props.editable) {
      // For adding a new item
      if (this.props.add) {
        return (
          <ListItem disabled={true}>
            <div>
              <TextField
                name="key"
                floatingLabelText="Key"
                style={styles.keyField}
                onChange={this.handleKeyChange}
                disabled={this.state.dirty === deleted}
                errorText={this.state.keyError}
                value={this.state.keyValue}
                />
              <TextField
                name="value"
                floatingLabelText="Value"
                style={styles.keyField}
                onChange={this.handleValueChange}
                disabled={this.state.dirty === deleted}
                errorText={this.state.valueError}
                value={this.state.valueValue}
                />
              <FlatButton
                style={styles.rightAllign}
                label="Add Key Pair"
                secondary={true}
                onTouchTap={this.createNew}/>
            </div>
          </ListItem>
        )
      }
      // Render editable field
      return (
        <ListItem disabled={true}>
          <div>
            <TextField
              name="key"
              floatingLabelText="Key"
              defaultValue={this.key}
              style={styles.keyField}
              onChange={this.handleKeyChange}
              disabled={this.state.dirty === deleted || this.state.dirty === newDeleted}
              />
            <TextField
              name="value"
              floatingLabelText="Value"
              style={styles.keyField}
              defaultValue={this.value}
              onChange={this.handleValueChange}
              disabled={this.state.dirty === deleted || this.state.dirty === newDeleted}
              />
            <FlatButton
              icon={this.state.dirty === deleted ? <Undo/> : <Delete />}
              style={styles.rightAllign}
              onTouchTap={this.toggleDeleted}
              />
            <FlatButton
              icon={this.state.stale ? <Error/> : <ErrorOutline />}
              style={styles.rightAllign}
              onTouchTap={this.toggleStale}
              />
          </div>
        </ListItem>
      );
    } else {
      // Render viewable field
      var functions = [];
      var self = this;

      if (this.state.dirty === dirty) {
        // Update on server if changes have been made.
        functions.push(function (cb) {
          self.serverRequest = $.ajax(config.apiHost + 'Items/' + self.props.childId, {
            method: 'PUT',
            data: JSON.stringify({
              "Id": parseInt(self.props.childId),
              "Key": self.key,
              "Value": self.value,
              "Parent": self.props.parentId,
              "Stale": self.state.stale,
              "Type": "leaf"
            }),
            headers: {
              'Content-Type': 'application/json'
            },
            complete: function (result) {
              if (result.status !== 200) {
                console.error(result);
              }
              cb();
            },
          });
        });
        this.setState({ dirty: clean });
      } else if (this.state.dirty === deleted) {
        // Send delete request if deleted.
        this.setState({ dirty: clean });
        functions.push(function (cb) {
          self.serverRequest = $.ajax(config.apiHost + 'Items/' + self.props.childId, {
            method: 'DELETE',
            complete: function (result) {
              if (result.status !== 200) {
                console.error(result);
              }
              cb();
            }
          });
        });

      } else if (this.state.dirty === neww ) {
        if(!(this.state.keyValue && this.state.valueValue && this.props.add)){
          return(<div></div>)
        }
        // Send post request if new.
        this.setState({ dirty: clean });
        functions.push(function (cb) {
          self.serverRequest = $.ajax(config.apiHost + 'Items/', {
            method: 'POST',
            data: JSON.stringify({
              "Key": self.key,
              "Value": self.value,
              "Parent": self.props.parentId,
              "Stale": self.state.stale,
              "Type": "leaf"
            }),
            headers: {
              'Content-Type': 'application/json'
            },
            complete: function (result) {
              var status = result.status + '';
              if (status.substring(0, 1) !== '2') {
                console.error(result);
              }
              cb();
            }
          });
        });
      }
      if (functions.length > 0) {
        async.parallel(functions, function () {
          self.props.update();
        });
      }
      var hide = { display: 'block' }
      if (this.state.dirty === newDeleted) {
        hide = { display: 'none' }
      }

      var keyValue = this.isUrl(this.value) ? <a  style={{}} href={this.value} target="_blank">{this.value}</a> : this.value;
      const errorIcon = this.state.stale ? <Error /> : '';
      return (
        <div>
          <ListItem primaryText= { <p>{this.key} <span style={{color: lightBlack}}>: {keyValue} {errorIcon} </span></p>} rightIcon={<ContentCopy />} onTouchTap={this.copyToClipboard.bind(this)} style={hide} innerDivStyle={{padding: '10px 16px 10px'}}></ListItem>
          <Snackbar
            open={this.state.open}
            message={this.state.copy ? "Copied to clipboard" : "Couldn't copy to clipboard"}
            autoHideDuration={4000}
            onRequestClose={this.handleRequestClose}
            />
        </div>
      );
    }
  }
}
export default ModalField;
