CREATE TABLE [dbo].[ZonewiseCity] (
    [cityid] SMALLINT     NOT NULL,
    [City]   VARCHAR (50) NOT NULL,
    [state]  VARCHAR (50) NOT NULL,
    [Zone]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ZonewiseCity] PRIMARY KEY CLUSTERED ([cityid] ASC)
);

