function fireComscorePageView() {
    try {
        var comscoreApiUrl = "/api/comscore/";
        self.COMSCORE && COMSCORE.beacon({ c1: "2", c2: "19261200" });
        $.get(comscoreApiUrl);
    }
    catch (e) {
        Console.log(e.message);
    }
}