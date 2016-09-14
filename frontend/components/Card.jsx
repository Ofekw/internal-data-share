import React from 'react';
import {List, ListItem} from 'material-ui/List';
import {Card, CardActions, CardHeader, CardText} from 'material-ui/Card';
import FlatButton from 'material-ui/FlatButton';
import ModalField from './ModalField.jsx';

class CardExampleExpandable extends React.Component {
  constructor(props) {
    super(props);
    this.children = [];
    this.title = '';
    this.id = -1;
  }

  createNew = () => {
    this.children.push(<ModalField editable="true" key={Date.now()}/>);
    this.id = this.props.cardData.Id;
    this.forceUpdate();
  }

  render() {
    if(this.props.hide){
      return <div></div>
    }
    if(this.props.cardData) {
      if(this.props.cardData.Id !== this.id){
        this.children = [];
      }
      this.title = this.props.cardData.Key;
      const leafChildren = this.props.cardData.LeafChildren;
      for (var child in leafChildren) {
        if (leafChildren.hasOwnProperty(child) && this.props.cardData.Id !== this.id) {
          const childElement = leafChildren[child];
          this.children.push(
            <ModalField editable={this.props.editable} key={childElement.Id} identifier={childElement.Key} value={childElement.Value} />
          );
        }
      }
    }

    return (
      <Card>
        <CardHeader
          title={this.title}
          actAsExpander={false}
          showExpandableButton={false}
        />
        <List ref="theList">
          {this.children.map(function(child, index) {
            return child;
          })}
        </List>
          <CardActions>
          {(() => {
            if (this.props.editable){
              return <FlatButton label="New" onTouchTap={this.createNew}/>
            }            
          })()}
          </CardActions>
      </Card>

    );
  }
}
export default CardExampleExpandable;
