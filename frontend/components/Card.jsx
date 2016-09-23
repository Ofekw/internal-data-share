import React from 'react';
import {List, ListItem} from 'material-ui/List';
import {Card, CardActions, CardHeader, CardText} from 'material-ui/Card';
import FlatButton from 'material-ui/FlatButton';
import ModalField from './ModalField.jsx';

// Card for displaying information for an environemnt.
class CardExampleExpandable extends React.Component {
  constructor(props) {
    super(props);
    this.children = [];
    this.title = '';
    this.editable = false;
  }

  update = () => {
    this.props.handleClick(this.props.cardData,false);
  }

  // Add a new child.
  createNew = (key,value) => {
    this.editable = false;
    this.props.cardData.LeafChildren.pop();
    var uid = new Date().getTime();
    this.props.cardData.LeafChildren.push({
      'Key': key,
      'Value': value,
      'newId' : key + uid,
      'new' : true
    });
    this.forceUpdate();
  }

  render() {
    if (this.props.hide) {
      return <div></div>
    }

    if(this.props.cardData) {
      if(this.editable){
        this.props.cardData.LeafChildren.pop();
        this.editable = false;
      }
      // Edit mode
      if(this.props.editable){
        this.editable = true;
        this.props.cardData.LeafChildren.push({
          'Key': '',
          'Value': '',
          'add': true
        });
      }

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

    var itemStyle = {
			width: '100%',
			display: 'inline-block',
			position: 'relative'
		};

		var buttonStyle = {
			display: 'inline-block',
			position: 'relative',
		};

    return (
      <Card>
        <CardHeader
          title={this.title}
          actAsExpander={false}
          showExpandableButton={false}
          />
        <List ref="theList">
          {this.children.map(function (child, index) {
            // Add all the children.
            return child;
          }) }
        </List>
      </Card>

    );
  }
}

export default CardExampleExpandable;
