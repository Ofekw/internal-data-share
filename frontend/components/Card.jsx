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
    this.notes = [];
    this.title = '';
    //TODO: remove this
    this.id = -1;

    this.state = {
      nodeComment: ""
    }

  }
  componentDidMount() {
    this.getNotes();
  }

  getNotes() {
    var self = this;
    if (this.props.cardData != null) {
      this.serverRequest = $.ajax(config.apiHost + 'Items/' + this.props.cardData.Id, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json'
        },
        complete: function (result) {
          if (result.status !== 200) {
            console.error(result);
          } else {
            var response = JSON.parse(result.responseText)
            self.notes = response.Notes;
            self.forceUpdate();
          }
        },
      });
    }
  }

  // Add a new child.
  createNew = () => {
    this.props.cardData.LeafChildren.push({
      'Key': '',
      'Value': '',
      'new': true
    });
    this.forceUpdate();
  }

  // Add a new comment
  addNewNotes() {
    var self = this;
    var comment = $('#nodeComment').val();
    this.serverRequest = $.ajax(config.apiHost + 'Items/' + this.props.cardData.Id + '?noteContent=' + comment, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json'
      },
      complete: function (result) {
        if (result.status !== 200) {
          debugger;
          console.error(result);
        } else {
          debugger;
          var response = JSON.parse(result.responseText)
          self.notes.push({ Id: response, Content: comment });
          self.setState({ nodeComment: "" })
          //self.forceUpdate();
        }
      },
    });
  }

  noteChange(event) {
    this.setState({nodeComment:event.target.value})
  }

  render() {
    if (this.props.hide) {
      return <div></div>
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
            <ModalField new={childElement.new} editable={this.props.editable} key={childElement.Id} childId={childElement.Id} identifier={childElement.Key} value={childElement.Value} parentId={this.props.cardData.Id} />
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
          <List>
            <Subheader>Notes</Subheader>
            {
              this.notes.map(note => {
                // Toggles background colour if an item is going to be deleted
                // var icon = <Delete />;
                var style = {};
                // if (this.state.delete.indexOf(item.Id) > -1){
                // 	icon = <Undo />
                // 	style = {backgroundColor: '#ddd'}
                // }
                console.log(note);
                return <ListItem primaryText={note.Content} key={note.Id} disabled={true} style={style}/>
              })
            }
          </List>
        </div>
        <CardActions>
          {(() => {
            // Immediately invoked function to add "New" button if in editable mode.
            if (this.props.editable) {
              return <div>
                <div style={divStyle}>
                  <FlatButton label="Add Label" secondary={true}  onTouchTap={this.createNew}/>
                  <br/>
                  <div>
                  </div>
                </div>
                <div style={divStyle}>
                  <TextField id="nodeComment" ref="nodeComment" style={itemStyle} hintText="Comment" multiLine={true} value={this.state.nodeComment} onChange={this.noteChange.bind(this)}/>
                  <FlatButton label="Add Comment" style={buttonStyle} primary={true} onTouchTap={this.addNewNotes.bind(this) } />
                </div>
              </div>
            }
          })() }
        </CardActions>
      </Card >

    );
  }
}

export default CardExampleExpandable;
