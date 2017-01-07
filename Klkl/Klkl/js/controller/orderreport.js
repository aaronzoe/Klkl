var fileBrowserModule = angular.module('angle', ['agGrid']);

fileBrowserModule.controller('OrderReportController', function ($scope, $http, $filter, $state, ngDialog) {

    var columnDefs = [
        // this row just shows the row index, doesn't use any data from the row
        //{
        //    headerName: "#",
        //    width: 50,
        //    cellRenderer: function(params) {
        //        return params.node.id + 1;
        //    }
        //},
        {
            headerName: '订单号',
            field: 'OrderID',
            width: 100,
            filter: 'text'

            // template: '<div class="ngCellText" ng-class="col.colIndex()"><a ui-serf="app.order-view({id:{{row.getProperty(col.ID)}}})">{{row.getProperty(col.OrderID)}}</a></div>'
            // template: '<span style="font-weight: bold;" ng-bind="data.OrderID"></span>'
        },
        { headerName: '客户渠道', field: 'Khqd', width: 100, filter: 'text' },
        { headerName: '客户名称', field: 'Khmc', width: 100, filter: 'set' },
        { headerName: '联系人', field: 'Lxr', width: 100, filter: 'text' },
        { headerName: '联系电话', field: 'Lxdh', width: 100, filter: 'text' },
        { headerName: '区域', field: 'AreaName', width: 100, filter: 'set' },
        { headerName: '客户代表', field: 'Khdb', width: 100 },
        {
            headerName: '打款金额',
            valueGetter: function(data) {
                return data.data.Zje - data.data.Dk - data.data.Yf <= 150 ? '<span class="label label-success">' + data.data.Dk + '</span>' : '<span class="label label-danger">' + data.data.Dk + '</span>';
            },
            width: 100,
            filter: 'number'
        },
        { headerName: '发货总金额', field: 'Zje', width: 100, filter: 'number' },
        { headerName: '费用金额', field: 'Fy', width: 100, filter: 'number' },
        { headerName: '运费金额', field: 'Yf', width: 100, filter: 'number' },
        { headerName: '备注', field: 'Remark', filter: 'text' },
        { headerName: '录入时间', field: 'CreateAt', filter: 'text' },
        //{
        //    headerName: '状态',
        //    valueGetter: function(data) {
        //        return data.data.Zje - data.data.Dk <= 0 ? '<span class="label label-success">正常</span>' : '<span class="label label-danger">发货金额大于打款</span>';
        //    },
        //    filter: 'text'
        //}
    ];
    $scope.pageSize = '9999';

    $scope.gridOptions = {
        // note - we do not set 'virtualPaging' here, so the grid knows we are doing standard paging
        enableSorting: true,
        enableFilter: true,
        enableColResize: true,
        columnDefs: columnDefs,
        rowHeight: 33,
        headerHeight: 33,
        onRowSelected: rowSelectedFunc,
        rowSelection: 'single',
        onAfterFilterChanged: function () {
            $scope.Dkje = 0;
            $scope.Fhzje = 0;
            $scope.Fyje = 0;
            $scope.Yfje = 0;
            $scope.gridOptions.api.forEachNodeAfterFilter(SumJe);

            $scope.Dkje = $scope.Dkje.toFixed(2);
            $scope.Fhzje = $scope.Fhzje.toFixed(2);
            $scope.Fyje = $scope.Fyje.toFixed(2);
            $scope.Yfje = $scope.Yfje.toFixed(2);
        }

    };


    function SumJe(node, index) {
   
        $scope.Dkje += node.data.Dk;
        $scope.Fhzje += node.data.Zje;
        $scope.Fyje += node.data.Fy;
        $scope.Yfje += node.data.Yf;
    }


    function rowSelectedFunc(event) {

        var url = $state.href("app.order-view", { id: event.node.data.ID });
        window.open(url, '_blank');
  
    //    $state.go("app.order-view", { id: event.node.data.ID });
        //     window.alert("row " + event.node.data + " selected");
    }

   


    $scope.onPageSizeChanged = function () {
        createNewDatasource();
    };

    var allOfTheData;
    $scope.loadData = function () {
        $http.post('/order/getorderreport', { Dt1: $scope.dt1, Dt2: $scope.dt2 })
            .then(function (res) {
                allOfTheData = res.data;
                createNewDatasource();
          //      $scope.gridOptions.api.setRowData(allOfTheData);
                $scope.Dkje = $scope.sum(allOfTheData, "Dk");
                $scope.Fhzje = $scope.sum(allOfTheData, "Zje");
                $scope.Fyje = $scope.sum(allOfTheData, "Fy");
                $scope.Yfje = $scope.sum(allOfTheData, "Yf");
            });

    };

    var d = new Date();
    $scope.dt1 = $filter('date')(d, 'yyyy-01-01');
    $scope.dt2 = $filter('date')(d, 'yyyy-12-31');
    $scope.selectYear = parseInt($filter('date')(d, 'yyyy'));
    $scope.changeYear = function (year) {
        $scope.selectYear = year;
        $scope.dt1 = year + '-01-01';
        $scope.dt2 = year + '-12-31';
        $scope.loadData();
    };
    $scope.loadData();

    $scope.years = [parseInt($filter('date')(d, 'yyyy')) - 2, parseInt($filter('date')(d, 'yyyy')) - 1, parseInt($filter('date')(d, 'yyyy'))];
    //$scope.years.add();
    //$scope.years.add();
    //$scope.years.add();
    function createNewDatasource() {
        if (!allOfTheData) {
            // in case user selected 'onPageSizeChanged()' before the json was loaded
            return;
        }

        var dataSource = {
            //rowCount: ???, - not setting the row count, infinite paging will be used
            pageSize: parseInt($scope.pageSize), // changing to number, as scope keeps it as a string
            getRows: function(params) {
                // this code should contact the server for rows. however for the purposes of the demo,
                // the data is generated locally, a timer is used to give the experience of
                // an asynchronous call
                setTimeout(function() {
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
            },
            rowCount: allOfTheData.length
    };
        $scope.gridOptions.api.setDatasource(dataSource);

    
       
    }
    $scope.open = function ($event, o) {
        $event.preventDefault();
        $event.stopPropagation();
        if (o === 1) {
            $scope.opened1 = true;
        }
        else
            $scope.opened2 = true;

    };


});

