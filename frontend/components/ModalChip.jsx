import React from 'react';
import Chip from 'material-ui/Chip'
import $ from 'jquery';
import config from '../config.js';

const styles = {
  chip: {
    margin: '4px'
  }
};

const clean = 'clean';
const deleted = 'deleted';
const neww = 'new';

// Key value text field that can be toggled between view and edit modes.
class ModalChip extends React.Component {
  constructor(props) {
    super(props);

    this.value = props.value;

    if (this.props.new) {
      console.log('constructor called on new chip');
    }

    var dirty = this.props.new ? neww : clean;

    this.state = {
      dirty: dirty,
      visible: true,
      editable: props.editable
    };
  }

  deleteChip = () => {
    this.setState({
      dirty: deleted,
      visible: false
    });
  }

  render() {
    if (this.props.editable) {
      // Render editable field
      if (this.state.visible) {
        return (
          <Chip
            onRequestDelete={this.deleteChip}
            key={this.props.identifier}
            style={styles.chip}
          >
            {this.props.value}
          </Chip>
        );
      } else {
        return null;
      }
    } else {
      // Render viewable field
      if (this.state.dirty === deleted) {
        // Send delete request if deleted.
        this.setState({dirty: clean});
        this.serverRequest = $.ajax(config.apiHost + 'Items/' + this.props.parentId + '/?labelId=' + this.props.identifier, {
          method: 'DELETE',
          complete: function (result) {
            if (result.status !== 200){
              console.error(result);
            }
          }
        });
      } else if (this.state.dirty === neww) {
        // Send delete request if deleted.
        this.setState({dirty: clean});
        this.serverRequest = $.ajax(config.apiHost + 'Items/' + this.props.parentId + '/?labelId=' + this.props.identifier, {
          method: 'PUT',
          data: JSON.stringify({}),
          headers: {
            'Content-Type': 'application/json'
          },
          complete: function (result) {
            if (result.status !== 200){
              console.error(result);
            }
          }
        });
      }
      if (this.state.visible) {
        return (
          <Chip
            key={this.props.identifier}
            style={styles.chip}
          >
            {this.props.value}
          </Chip>
        );
      } else {
        return null;
      }
    }
  }
}
export default ModalChip;
