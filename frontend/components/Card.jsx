import React from 'react';
import {List, ListItem} from 'material-ui/List';
import {Card, CardActions, CardHeader, CardText} from 'material-ui/Card';
import FlatButton from 'material-ui/FlatButton';
import ModalField from './ModalField.jsx';
import TextField from 'material-ui/TextField';
import Subheader from 'material-ui/Subheader';
import $ from 'jquery';
import config from '../config.js';

// Card for displaying information for an environemnt.
class CardExampleExpandable extends React.Component {
  constructor(props) {
    super(props);
    this.children = [];
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

  // Add a new child.
  createNew = (key, value) => {
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

    if (this.props.cardData) {
      if (this.editable) {
        this.props.cardData.LeafChildren.pop();
        this.editable = false;
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

      if (this.state.notesDirty && !this.props.editable) {
        this.addNewNotes();
      }

      if (this.props.cardData) {
        this.title = this.props.cardData.Key;
        const leafChildren = this.props.cardData.LeafChildren;

        this.children = [];

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
                createNew = {this.createNew}
                update={this.update}/>
            );
          }
        }
      }

      var divStyle = {
        display: 'flex',
      };

      var itemStyle = {
        marginLeft: 10,
        width: '85%',
        display: 'inline-block',
        position: 'relative'
      };

      var buttonStyle = {
        display: 'inline-block',
        position: 'relative',
        width: '150px'
      };

      var textArea =
        <div style={divStyle}>
          <TextField floatingLabelText="Note" id="nodeComment" disabled={!this.props.editable} ref="nodeComment" style={itemStyle} hintText="Note" multiLine={true} value={this.state.nodeComment} onChange={this.noteChange.bind(this) }/>
        </div>


      return (
        <Card>
          <CardHeader
            title={this.title}
            actAsExpander={false}
            showExpandableButton={false}
            />
          <div>
            <List ref="theList">
              {this.children.map(function (child, index) {
                // Add all the children.
                return child;
              }) }
            </List>
          </div>
          <CardActions>
            return <div> {textArea} </div>
          </CardActions>
        </Card >

      );
    }
  }
}

export default CardExampleExpandable;
