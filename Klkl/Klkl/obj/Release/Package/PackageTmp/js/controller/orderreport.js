var fileBrowserModule = angular.module('angle', ['agGrid']);

fileBrowserModule.controller('OrderReportController', function ($scope, $http, $filter) {

    var columnDefs = [
        // this row just shows the row index, doesn't use any data from the row
        //{
        //    headerName: "#",
        //    width: 50,
        //    cellRenderer: function(params) {
        //        return params.node.id + 1;
        //    }
        //},
        { headerName: '订单号', field: 'OrderID', width: 100, filter: 'text' },
        { headerName: '客户渠道', field: 'Khqd', width: 100, filter: 'text' },
        { headerName: '客户名称', field: 'Khmc', width: 100, filter: 'set' },
        { headerName: '联系人', field: 'Lxr', width: 100, filter: 'text' },
        { headerName: '联系电话', field: 'Lxdh', width: 100, filter: 'text' },
        { headerName: '区域', field: 'AreaName', width: 100, filter: 'set' },
        { headerName: '发货状态', field: 'Zt', width: 100 },
        { headerName: '打款金额', field: 'Dk', width: 100, filter: 'number' },
        { headerName: '发货总金额', field: 'Zje', width: 100, filter: 'number' },
        { headerName: '费用金额', field: 'Fy', width: 100, filter: 'number' },
        { headerName: '运费金额', field: 'Yf', width: 100, filter: 'number' },
        { headerName: '备注', field: 'Remark', filter: 'text' },
        { headerName: '录入时间', field: 'CreateAt', filter: 'text' }
    ];    $scope.pageSize = '15';    $scope.gridOptions = {
        // note - we do not set 'virtualPaging' here, so the grid knows we are doing standard paging
        enableSorting: true,
        enableFilter: true,
        enableColResize: true,
        columnDefs: columnDefs,
        rowHeight: 33,
        headerHeight:33
    };    $scope.onPageSizeChanged = function () {
        createNewDatasource();
    };
    var allOfTheData;
    $scope.loadData = function () {
        $http.post('/order/getorderreport', { Dt1: $scope.dt1, Dt2: $scope.dt2 })
            .then(function (res) {
                allOfTheData = res.data;
                createNewDatasource();

                $scope.Dkje = $scope.sum(allOfTheData, "Dk");
                $scope.Fhzje = $scope.sum(allOfTheData, "Zje");
                $scope.Fyje = $scope.sum(allOfTheData, "Fy");
                $scope.Yfje = $scope.sum(allOfTheData, "Yf");
            });

    };
    var d = new Date();    $scope.dt1 = $filter('date')(d, 'yyyy-01-01');
    $scope.dt2 = $filter('date')(d, 'yyyy-12-31');
    $scope.loadData();
    function createNewDatasource() {
        if (!allOfTheData) {
            // in case user selected 'onPageSizeChanged()' before the json was loaded
            return;
        }

        var dataSource = {
            //rowCount: ???, - not setting the row count, infinite paging will be used
            pageSize: parseInt($scope.pageSize), // changing to number, as scope keeps it as a string
            getRows: function (params) {
                // this code should contact the server for rows. however for the purposes of the demo,
                // the data is generated locally, a timer is used to give the experience of
                // an asynchronous call
                setTimeout(function () {
                    // take a chunk of the array, matching the start and finish times
                    var rowsThisPage = allOfTheData.slice(params.startRow, params.endRow);
                    // see if we have come to the last page. if we have, set lastRow to
                    // the very last row of the last page. if you are getting data from
                    // a server, lastRow could be returned separately if the lastRow
                    // is not in the current page.
                    var lastRow = -1;
                    if (allOfTheData.length <= params.endRow) {
                        lastRow = allOfTheData.length;
                    }
                    params.successCallback(rowsThisPage, lastRow);
                }, 50);
            }
        };
        $scope.gridOptions.api.setDatasource(dataSource);
       
    }



});