/*!
 * FusionCharts updateChartXMLAttribute
 * Copyright (c) 2010 FusionCharts
 * http://www.fusioncharts.com/
 *
 * @publish: May 30th, 2010
 * @version: 1.0.0
 *
 */

(function () {

    // Proceed with script only if FusionCharts JS object is available.
    if (typeof FusionCharts === 'undefined') {
        throw 'Unable to locate FusionCharts JavaScript object.';
    }

	/**
	 * Updates a FusionCharts DataXML root's attribute with the new
	 * attribute-value pair. In case the attribute does not exist, it adds
	 * it.
	 *
	 * @param xml {String} The source FusionCharts DataXML.
	 * @param attribute {String} The attribute to be updated.
	 * @param value {String} The new value for the attribute to be updated.
	 *
	 * @type String
	 * @return Updated FusionCharts DataXML with the new attribute added or
	 * updated
	 */
	var updateXML = function (xml, attribute, value) {

		// In case user sends multiple number of attributes in
		// {attribute: value} format, then loop through the attributes
		// and update xml
		if (typeof attribute === 'object' && typeof value === 'undefined') {
			for (var item in attribute) {
				xml = updateXML(xml, item, attribute[item]);
			}
			return xml;
		}

		// Create a RegExp that would extract the attribute as provided
		attribute = attribute.toLowerCase();
		var r = new RegExp("\\s" + attribute + "=\\\"[^\"]+?\\\"|" +
			attribute + "=\\\'[^']+?\\\'", 'ig'),

			// Create the replacement string for the attribute like
			// attribute= 'value'
			v = ' ' + attribute + '=\"' + 
				value.toString().replace(/\"/ig, '&quot;') + '\"';

		// Check whether the attribute already exists in XML. If it exists
		// then do a replace, else add a new attribute
		return r.test(xml) ?
			xml.replace(r, v) :
			xml.replace(/(<\w+?)\s+?/, '$1' + v + ' ');
	};
	
	/**
	 * Updates a FusionCharts object's XML root's attribute with the new
	 * attribute-value pair. In case the attribute does not exist, it adds
	 * it.
	 *
	 * @param chart {String} The source FusionCharts DOM ID.
	 * @param attribute {String} The attribute to be updated.
	 * @param value {String} The new value for the attribute to be updated.
	 * @param silent {Boolean} Does not update the chart, but returns new XML
	 *
	 * @type String
	 * @return Updated FusionCharts DataXML with the new attribute added or
	 * updated
	 */
	FusionCharts.updateXMLAttribute = function(chart, attribute, value, silent) {
	
		// Get the chart SWF object
		if (typeof chart === 'string') {
			chart = getChartFromId(chart);
		}
		
		// Verify whether the chart is valid object and then proceed.
		if (!(chart && chart.getXML)) {
			throw "FusionCharts.updateChartAttributes:ArgumentException() Object not valid.";
		}
		
		// Get update chart XML by calling the relevant methods.
		var xml = updateXML(chart.getXML(), attribute, value);
		
		// If user does not specify that the operation is silent, then
		// update chart's XML.
		if (silent !== true) {
			chart.setDataXML(xml);
		}
		
		return xml;
	
	};

}());