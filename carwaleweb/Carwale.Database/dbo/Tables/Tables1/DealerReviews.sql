CREATE TABLE [dbo].[DealerReviews] (
    [Id]                NUMERIC (18)   IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CustomerId]        NUMERIC (18)   NOT NULL,
    [DealerId]          NUMERIC (18)   NOT NULL,
    [ProductRating]     SMALLINT       NOT NULL,
    [FinancePlans]      SMALLINT       NOT NULL,
    [ServiceAndSupport] SMALLINT       NOT NULL,
    [StaffCourtesy]     SMALLINT       NOT NULL,
    [Timeliness]        SMALLINT       NOT NULL,
    [Pros]              VARCHAR (100)  NULL,
    [Cons]              VARCHAR (100)  NULL,
    [Comments]          VARCHAR (4000) NULL,
    [Title]             VARCHAR (100)  NULL,
    [EntryDateTime]     DATETIME       NOT NULL,
    [isVarified]        BIT            CONSTRAINT [DF_DealerReviews_isVarified] DEFAULT (0) NOT NULL,
    [ReportAbuse]       BIT            CONSTRAINT [DF_DealerReviews_ReportAbuse] DEFAULT (0) NOT NULL,
    [Liked]             INT            CONSTRAINT [DF_DealerReviews_Liked] DEFAULT (0) NOT NULL,
    [Disliked]          INT            CONSTRAINT [DF_DealerReviews_Disliked] DEFAULT (0) NOT NULL,
    [Viewed]            INT            CONSTRAINT [DF_DealerReviews_Viewed] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_DealerReviews] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO

--THIS TRIGGER IS TO update the review rate and the count for the version and model table

CREATE TRIGGER [TrigDealerReviewRate] ON dbo.DealerReviews 

FOR INSERT
 	
AS
	DECLARE 	@ReviewId			NUMERIC,
			@DealerId			NUMERIC,
			@ReviewRate  	 		DECIMAL(10,2),
			@ReviewCount 			INT,
			@AvgFinancePlans		DECIMAL(10,2),
			@AvgServiceAndSupport	DECIMAL(10,2),
			@AvgStaffCourtesy		DECIMAL(10,2),
			@AvgTimeliness			DECIMAL(10,2),
			@IsDealerIdExist		INT
					
BEGIN
	--get hte id
	SET @ReviewId = IDENT_CURRENT('DealerReviews')

	SELECT @DealerId = DealerId  FROM DealerReviews WHERE Id = @ReviewId
	
	--fetch the modelid, versionid, overallrate
	SELECT 
		@ReviewRate = IsNull(AVG(ProductRating), '0'),
		@AvgFinancePlans = IsNull(AVG(FinancePlans), '0'),
		@AvgServiceAndSupport = IsNull(AVG(ServiceAndSupport), '0'),
		@AvgStaffCourtesy = IsNull(AVG(StaffCourtesy), '0'),
		@AvgTimeliness = IsNull(AVG(Timeliness), '0'),
		@ReviewCount = Count(Id) 
	FROM
		DealerReviews
	WHERE
		DealerId = @DealerId  

	Select DealerId From DealersReviewCount Where DealerId = @DealerId

	SET @IsDealerIdExist = @@ROWCOUNT

	IF @IsDealerIdExist = 0

		INSERT INTO DealersReviewCount(DealerId, ReviewRate, ReviewCount, AvgFinancePlans, 
				AvgServiceAndSupport, AvgStaffCourtesy, AvgTimeliness) 
		VALUES(@DealerId, @ReviewRate, @ReviewCount, @AvgFinancePlans, @AvgServiceAndSupport,
				@AvgStaffCourtesy, @AvgTimeliness)
		
	ELSE
		UPDATE DealersReviewCount 
		SET 	ReviewRate = @ReviewRate,
			ReviewCount = @ReviewCount,
			AvgFinancePlans = @AvgFinancePlans, 
			AvgServiceAndSupport = @AvgServiceAndSupport,
			AvgStaffCourtesy = @AvgStaffCourtesy,
			AvgTimeliness = @AvgTimeliness
		WHERE DealerId = @DealerId

END










