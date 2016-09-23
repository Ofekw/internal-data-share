import React from 'react';
import $ from 'jquery';
import {List, ListItem} from 'material-ui/List';
import {Card, CardActions, CardHeader, CardText} from 'material-ui/Card';
import FlatButton from 'material-ui/FlatButton';
import Dialog from 'material-ui/Dialog';
import SelectField from 'material-ui/SelectField';
import MenuItem from 'material-ui/MenuItem';
import TextField from 'material-ui/TextField';
import Subheader from 'material-ui/Subheader';
import config from '../config.js';
import Chip from 'material-ui/Chip';
import ModalField from './ModalField.jsx';
import ModalChip from './ModalChip.jsx';
import labelManager from '../labelManager.js'

const styles = {
  wrapper: {
    display: 'flex',
    flexWrap: 'wrap',
  }
}

// Card for displaying information for an environemnt.
class CardExampleExpandable extends React.Component {
  constructor(props) {
    super(props);
    this.children = [];
    this.labels = [];
    this.title = '';
    this.editable = false;
    if (props.cardData != null) {
      this.state = {
        nodeComment: props.cardData.Note == null ? "" : props.cardData.Note,
        notesDirty: false
      }
    }
  }

  update = () => {
    this.props.handleClick(this.props.cardData, false);
  }

  noteChange(event) {
    this.props.cardData.Note = event.target.value;
    this.setState({ nodeComment: event.target.value, notesDirty: true })
  }

  state = {
    open: false,
  };

  componentDidMount = () => {
    var self = this;
    labelManager.getLabels(function (labels) {
      self.setState({
        labels: labels
      });
    }.bind(this));
  }

  handleLabelDialogOpen = () => {
    this.setState({open: true});
  };

  handleLabelDialogClose = () => {
    this.setState({open: false});
  };

  handleLabelDialogSave = () => {
    this.setState({open: false});
    this.createNewLabel();
  };

  // Add a new child.
  createNewField = (key,value) => {
    this.editable = false;
    this.props.cardData.LeafChildren.pop();
    var uid = new Date().getTime();

    this.props.cardData.LeafChildren.push({
      'Key': key,
      'Value': value,
      'newId': key + uid,
      'new': true
    });
    this.forceUpdate();
  };

  // Add a new child.
  createNewLabel = () => {
    if (this.nextLabel) {
      this.props.cardData.Labels.push(this.nextLabel);
      this.nextLabel = undefined;
    }
    this.forceUpdate();
  };

  handleLabelChange = (event, index, value) => {
    this.setState({selectValue: value});
    this.nextLabel = {
      'Id': value,
      'Content': labelManager.getLabelById(value).Content,
      new: true
    };
  }

  // Add a new comment
  addNewNotes() {
    var self = this;
    var comment = $('#nodeComment').val();
    this.serverRequest = $.ajax(config.apiHost + 'Items/' + this.props.cardData.Id + '/Note', {
      method: 'PUT',
      data: JSON.stringify(this.state.nodeComment),
      headers: {
        'Content-Type': 'application/json'
      },
      success: function (result) {
      },
      failure: function (result) {
        console.log(result);
      },
    });
  }

  render() {
    if (this.props.hide) {
      return <div></div>
    }

    if (this.state.notesDirty && !this.props.editable) {
      this.addNewNotes();
    }

    if (this.props.cardData) {
      if (this.editable) {
        this.props.cardData.LeafChildren.pop();
      }
      // Edit mode
      if (this.props.editable) {
        this.editable = true;
        this.props.cardData.LeafChildren.push({
          'Key': '',
          'Value': '',
          'add': true
        });
      }

      this.title = this.props.cardData.Key;
      const leafChildren = this.props.cardData.LeafChildren;
      const labels = this.props.cardData.Labels;

      this.children = [];
      this.labels = [];

      for (var child in leafChildren) {
        // Add all the children.
        if (leafChildren.hasOwnProperty(child)) {
          const childElement = leafChildren[child];
          this.children.push(
            <ModalField new={childElement.new}
            add = {childElement.add}
            editable={this.props.editable}
            key={childElement.Id || childElement.newId || childElement.add}
            childId={childElement.Id}
            identifier={childElement.Key}
            value={childElement.Value}
            parentId={this.props.cardData.Id}
            createNew = {this.createNewField}
            update={this.update}/>
          );
        }
      }

      for (var label in labels) {
        const labelElement = labels[label];
        this.labels.push(
          <ModalChip
            editable={this.props.editable}
            key={labelElement.Id}
            identifier={labelElement.Id}
            value={labelElement.Content}
            parentId={this.props.cardData.Id}
            new={labelElement.new}
            />
        )
      }
    }

    const divStyle = {
      display: 'flex',
    };

    const itemStyle = {
      marginLeft: 10,
      width: '85%',
      display: 'inline-block',
      position: 'relative'
    };

    const buttonStyle = {
      display: 'inline-block',
      position: 'relative',
      width: '150px'
    };

    const textArea =
      <div style={divStyle}>
        <TextField floatingLabelText="Note" id="nodeComment" disabled={!this.props.editable} ref="nodeComment" style={itemStyle} hintText="Note" multiLine={true} value={this.state.nodeComment} onChange={this.noteChange.bind(this) }/>
      </div>

    const actions = [
      <FlatButton
        label="Cancel"
        primary={true}
        onTouchTap={this.handleLabelDialogClose}
        />,
      <FlatButton
        label="Create"
        primary={true}
        keyboardFocused={true}
        onTouchTap={this.handleLabelDialogSave}
        />,
    ];

    return (
      <Card>
        <CardHeader
          title={this.title}
          actAsExpander={false}
          showExpandableButton={false}
          />
        <div style={styles.wrapper}>
          {this.labels.map(function (label, index) {
            // Add the labels
            return label;
          })}
          {(() => {
            // Immediately invoked function to add "New" button if in editable mode.
            if (this.props.editable) {
              return (
                <span>
                  <FlatButton style={buttonStyle} label="Add Label" secondary={true}  onTouchTap={this.handleLabelDialogOpen}/>
                  <Dialog
                    title="Dialog With Actions"
                    actions={actions}
                    modal={false}
                    open={this.state.open}
                    onRequestClose={this.handleLabelDialogClose}
                    >
                    <div>
                      <SelectField value={this.state.selectValue} onChange={this.handleLabelChange}>
                        {this.state.labels && this.state.labels.map(function (label) {
                          return (<MenuItem key={label.Id} value={label.Id} primaryText={label.Content} />);
                        })}
                      </SelectField>
                    </div>
                  </Dialog>
                </span>
              );
            }
          })() }
        </div>
        <List ref="theList">
          {this.children.map(function (child, index) {
            // Add all the children.
            return child;
          }) }
        </List>
        <CardActions>
          return <div> {textArea} </div>
        </CardActions>
      </Card>

    );
  }
}

export default CardExampleExpandable;
