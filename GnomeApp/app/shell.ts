import router = require('plugins/router');

class Shell {
    router: DurandalRouter = router;
    activate = () => {
        var configs: DurandalRouteConfiguration[] = [
            { route: '',            moduleId: 'home/index',     title: 'Home',      nav: 1,     settings: { icon: 'fa fa-home fa-lg' } },
            { route: 'gnome',       moduleId: 'gnome/index',    title: 'Gnomes',    nav: 2,     settings: { icon: 'fa fa-users fa-lg' } },
            { route: 'military',    moduleId: 'military/index', title: 'Military',  nav: 3,     settings: { icon: 'fa fa-shield fa-lg' } },
            { route: 'world',       moduleId: 'world/index',    title: 'World',     nav: 4,     settings: { icon: 'fa fa-map-o fa-lg' } },
            { route: 'test',        moduleId: 'test/index',     title: 'Test',      nav: 99,     settings: { icon: 'fa fa-bolt fa-lg' } }
        ];

        return router.map(configs).buildNavigationModel().mapUnknownRoutes('home/index', 'not-found').activate();
    };
}

export = Shell;