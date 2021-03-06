/**
    * @ngdoc service
    * @name umbraco.resources.stylesheetResource
    * @description service to retrieve available stylesheets
    * 
    *
    **/
function stylesheetResource($q, $http, umbRequestHelper) {

    //the factory object returned
    return {
        
        /**
         * @ngdoc method
         * @name umbraco.resources.stylesheetResource#getAll
         * @methodOf umbraco.resources.stylesheetResource
         *
         * @description
         * Gets all registered stylesheets
         *
         * ##usage
         * <pre>
         * stylesheetResource.getAll()
         *    .then(function(stylesheets) {
         *        alert('its here!');
         *    });
         * </pre> 
         * 
         * @returns {Promise} resourcePromise object containing the stylesheets.
         *
         */
        getAll: function () {            
            return umbRequestHelper.resourcePromise(
               $http.get(
                   umbRequestHelper.getApiUrl(
                       "stylesheetApiBaseUrl",
                       "GetAll")),
               'Failed to retreive stylesheets ');
        },

        /**
         * @ngdoc method
         * @name umbraco.resources.stylesheetResource#getRules
         * @methodOf umbraco.resources.stylesheetResource
         *
         * @description
         * Returns all defined child rules for a stylesheet with a given ID
         *
         * ##usage
         * <pre>
         * stylesheetResource.getRules(2345)
         *    .then(function(rules) {
         *        alert('its here!');
         *    });
         * </pre> 
         * 
         * @returns {Promise} resourcePromise object containing the rules.
         *
         */
        getRules: function (id) {            
            return umbRequestHelper.resourcePromise(
               $http.get(
                   umbRequestHelper.getApiUrl(
                       "stylesheetApiBaseUrl",
                       "GetRules",
                       [{ id: id }])),
               'Failed to retreive stylesheets ');
        },

        /**
         * @ngdoc method
         * @name umbraco.resources.stylesheetResource#getRulesByName
         * @methodOf umbraco.resources.stylesheetResource
         *
         * @description
         * Returns all defined child rules for a stylesheet with a given name
         *
         * ##usage
         * <pre>
         * stylesheetResource.getRulesByName("ie7stylesheet")
         *    .then(function(rules) {
         *        alert('its here!');
         *    });
         * </pre> 
         * 
         * @returns {Promise} resourcePromise object containing the rules.
         *
         */
        getRulesByName: function (name) {            
            return umbRequestHelper.resourcePromise(
               $http.get(
                   umbRequestHelper.getApiUrl(
                       "stylesheetApiBaseUrl",
                       "GetRulesByName",
                       [{ name: name }])),
               'Failed to retreive stylesheets ');
        }
    };
}

angular.module('umbraco.resources').factory('stylesheetResource', stylesheetResource);
