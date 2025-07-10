/*!
 * FusionCharts updateChartXMLAttribute
 * Copyright (c) 2010 FusionCharts
 * http://www.fusioncharts.com/
 * @publish: May 30th, 2010
 * @version: 1.0.0
 */
(function(){if(typeof FusionCharts==='undefined'){throw'Unable to locate FusionCharts JavaScript object.';}var f=function(a,b,c){if(typeof b==='object'&&typeof c==='undefined'){for(var d in b){a=f(a,d,b[d])}return a}b=b.toLowerCase();var r=new RegExp("\\s"+b+"=\\\"[^\"]+?\\\"|"+b+"=\\\'[^']+?\\\'",'ig'),v=' '+b+'=\"'+c.toString().replace(/\"/ig,'&quot;')+'\"';return r.test(a)?a.replace(r,v):a.replace(/(<\w+?)\s+?/,'$1'+v+' ')};FusionCharts.updateXMLAttribute=function(a,b,c,d){if(typeof a==='string'){a=getChartFromId(a)}if(!(a&&a.getXML)){throw"FusionCharts.updateChartAttributes:ArgumentException() Object not valid.";}var e=f(a.getXML(),b,c);if(d!==true){a.setDataXML(e)}return e}}());