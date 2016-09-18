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
    this.id = -1;
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

  render() {
    if (this.props.hide) {
      return <div></div>
    }

    if(this.props.cardData) {
      this.title = this.props.cardData.Key;
      const leafChildren = this.props.cardData.LeafChildren;

      this.children = [];

      for (var child in leafChildren) {
        // Add all the children.
        if (leafChildren.hasOwnProperty(child)) {// && this.props.cardData.Id !== this.id) {
          const childElement = leafChildren[child];
          this.children.push(
            <ModalField new={childElement.new} editable={this.props.editable} key={childElement.Id} childId={childElement.Id} identifier={childElement.Key} value={childElement.Value} parentId={this.props.cardData.Id} />
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
        <CardActions>
          {(() => {
            // Immediately invoked function to add "New" button if in editable mode.
            if (this.props.editable) {
              return <div>
                <FlatButton style={buttonStyle} label="Add Label" secondary={true}  onTouchTap={this.createNew}/>
              </div>
            }
          })() }
        </CardActions>
      </Card>

    );
  }
}

export default CardExampleExpandable;
