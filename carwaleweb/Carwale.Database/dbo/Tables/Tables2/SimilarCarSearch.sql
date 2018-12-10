CREATE TABLE [dbo].[SimilarCarSearch] (
    [ID]            NUMERIC (18)    IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CarVersionId]  NUMERIC (18)    NULL,
    [Car]           VARCHAR (250)   NULL,
    [Price]         NUMERIC (18, 2) CONSTRAINT [DF_SimilarCarDetails_Price] DEFAULT (0) NULL,
    [PowerSteering] NUMERIC (18)    CONSTRAINT [DF_SimilarCarDetails_PowerSteering] DEFAULT (0) NULL,
    [AirCondition]  NUMERIC (18)    CONSTRAINT [DF_SimilarCarDetails_AirCondition] DEFAULT (0) NULL,
    [TransMission]  NUMERIC (18)    CONSTRAINT [DF_SimilarCarDetails_TransMission] DEFAULT (0) NULL,
    [Petrol]        NUMERIC (18)    CONSTRAINT [DF_SimilarCarDetails_Petrol] DEFAULT (0) NULL,
    [Diesel]        NUMERIC (18)    CONSTRAINT [DF_SimilarCarDetails_Diesel] DEFAULT (0) NULL,
    [Lpg]           NUMERIC (18)    CONSTRAINT [DF_SimilarCarDetails_Lpg] DEFAULT (0) NULL,
    [Luxury]        NUMERIC (18)    CONSTRAINT [DF_SimilarCarDetails_Luxury] DEFAULT (0) NULL,
    [Midsize]       NUMERIC (18)    CONSTRAINT [DF_SimilarCarDetails_Midsize] DEFAULT (0) NULL,
    [Sports]        NUMERIC (18)    CONSTRAINT [DF_SimilarCarDetails_Sports] DEFAULT (0) NULL,
    [Utility]       NUMERIC (18)    CONSTRAINT [DF_SimilarCarDetails_Utility] DEFAULT (0) NULL,
    [HatchBack]     NUMERIC (18)    CONSTRAINT [DF_SimilarCarDetails_HachBack] DEFAULT (0) NULL,
    [Minivan]       NUMERIC (18)    CONSTRAINT [DF_SimilarCarDetails_Minivan] DEFAULT (0) NULL,
    [MUV]           NUMERIC (18)    CONSTRAINT [DF_SimilarCarDetails_MUV] DEFAULT (0) NULL,
    [Sedan]         NUMERIC (18)    CONSTRAINT [DF_SimilarCarDetails_Sedan] DEFAULT (0) NULL,
    [StationWagon]  NUMERIC (18)    CONSTRAINT [DF_SimilarCarDetails_StationWagon] DEFAULT (0) NULL,
    [SUV]           NUMERIC (18)    CONSTRAINT [DF_SimilarCarDetails_SUV] DEFAULT (0) NULL,
    CONSTRAINT [PK_SimilarCarSearch] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

