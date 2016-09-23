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

    var dirty = this.props.new ? neww : clean;

    this.state = {
      dirty: dirty,
      open: false,
      editable: props.editable
    };
  }

  deleteChip = () => {
    this.setState({dirty: deleted});
  }

  render() {
    if (this.props.editable) {
      // Render editable field
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
        this.serverRequest = $.ajax(config.apiHost + 'Items/' + this.props.parentId + '/?labelId="' + this.props.identifier + '"', {
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
      return (
        <Chip
          key={this.props.identifier}
          style={styles.chip}
        >
          {this.props.value}
        </Chip>
      );
    }
  }
}
export default ModalChip;
