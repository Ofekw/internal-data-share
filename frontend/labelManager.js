import $ from 'jquery';
import config from './config.js';

var labels;

export default {
	getLabelById: function (id) {
		if (labels) {
			for (var i = 0; i < labels.length; i++) {
				if (labels[i].Id === id) {
					return labels[i];
				}
			}
		}
	},

	getLabels: function (callback) {
		if (!labels) {
			$.get(config.apiHost + 'labels', function (result) {
				labels = result;
				callback(labels);
			});
		} else {
			callback(labels);
		}
}
};
