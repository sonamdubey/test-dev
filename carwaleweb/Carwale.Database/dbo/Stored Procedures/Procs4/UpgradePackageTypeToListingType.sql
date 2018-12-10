IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpgradePackageTypeToListingType]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpgradePackageTypeToListingType]
GO

	-- =============================================
-- Author:        Amit Verma
-- Create date: 07/052013
-- Description:  Upgrade Package Type To Listing Type
-- modification: changed logic to upgrade package type by getting value from packages table by amit 10/31/2013
-- Modified by Avishkar 21-05-2014 to optimize the SP
-- Modified by Avishkar 07-07-2015 to avoid same InquiryID for a Dealer and an Individual Car
-- =============================================
CREATE PROCEDURE [dbo].[UpgradePackageTypeToListingType]


    -- Add the parameters for the stored procedure here
    @ConsumerType smallint,
    @CarId NUMERIC(18,0),
    @ConsumerId NUMERIC(18,0),
	@isOffline int = 0,
    @isUpgraded tinyint = 0 out
AS
BEGIN
   
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
   
    DECLARE @PackageType INT
    DECLARE @CityId INT = -1
    DECLARE @Validity int = -1
    DECLARE @ClassifiedValidity int
    DECLARE @PackageId int
    DECLARE @Amount int
    --DECLARE @DtExpiry DATETIME
   
    SET @isUpgraded = 0
   
    IF(@ConsumerType = 2)
    BEGIN
        DECLARE @PackageTypeTemp INT
       
	  --  SELECT @PackageTypeTemp = ISNULL(PackageType, 1),
   --         @CityId = (
   --             CASE
   --                 WHEN ISNULL(CityId,0) = 0
   --                     THEN - 1
   --                 ELSE CityId
   --                 END
   --             ),
			--@PackageId = PackageId
   --     FROM CustomerSellInquiries WITH (NOLOCK)
   --     WHERE id = @CarId

        -- Modified by Avishkar 21-05-2014 to optimize the SP
		SELECT @PackageTypeTemp = PackageType,
               @CityId = CityId,
			   @PackageId = PackageId
        FROM CustomerSellInquiries WITH (NOLOCK)
        WHERE id = @CarId

		SET @PackageTypeTemp = ISNULL(@PackageTypeTemp, 1)
		SET @CityId = ISNULL(@CityId,-1) 
                    
		
		
		SELECT @PackageType = InqPtCategoryId, @Amount = Amount FROM Packages WITH (NOLOCK) WHERE ID = @PackageId
   
        SELECT @PackageType 'PackageType',@CityId 'CityId',@Amount 'Amount',@PackageId PackageId
		        
		--if(@Amount != 0)
		----	SELECT @PackageId = PackageId FROM CustomerSellInquiries WITH (NOLOCK) WHERE ID = @CarId
		----ELSE
		--	SELECT TOP 1 @PackageId = PackageId
		--	FROM PGTransactions WITH (NOLOCK)
		--	WHERE CarId = @CarId
		--	ORDER BY Id DESC

		if(@Amount = 0 OR @isOffline = 1)
			SELECT @Validity = ISNULL(Validity, - 1),@ClassifiedValidity=ISNULL(ClassifiedValidity,-1)
			FROM Packages WITH (NOLOCK)
			WHERE ID = @PackageId
		ELSE
			SELECT @Validity = ISNULL(Validity, - 1),@ClassifiedValidity=ISNULL(ClassifiedValidity,-1)
			FROM Packages WITH (NOLOCK)
			WHERE ID = (    -- Modified by Avishkar 21-05-2014 to optimize the SP
				--SELECT Distinct  PackageId
				--FROM PGTransactions WITH (NOLOCK)
				--WHERE CarId = @CarId

				-- Modified by Avishkar 07-07-2015 to avoid same InquiryID for a Dealer and an Individual Car

				SELECT TOP 1 PackageId
				FROM PGTransactions WITH (NOLOCK)
				WHERE CarId = @CarId
				AND ConsumerType=@ConsumerType
				ORDER BY Id DESC
				)

        SELECT @Validity 'Validity'
       
        --SET @DtExpiry = CONVERT(DATE, DATEADD(dd,@Validity,GETDATE()))
        --SELECT @DtExpiry 'DtExpiry'
		DECLARE @isPremium BIT = (SELECT IsPremium FROM InquiryPointCategory WITH (NOLOCK) WHERE ID = @PackageType)
		--IF(@Amount > 0)
		--	SET @isPremium = 1

        IF(@CityId <> -1 AND @PackageTypeTemp <> @PackageType)
        BEGIN
            UPDATE CustomerSellInquiries
            SET PackageType = @PackageType,
                PackageExpiryDate = CONVERT(DATE, DATEADD(dd,@Validity,GETDATE())),
                ClassifiedExpiryDate= CONVERT(DATE, DATEADD(dd,@ClassifiedValidity,GETDATE())),
                IsApproved = 1,IsListingCompleted = 1,isPremium = @isPremium,ListingCompletedDate=GETDATE()
            WHERE ID = @CarId

            IF(@@ROWCOUNT <> 0)
                SET @isUpgraded = 1
        END
   
    END
   
    ELSE    
        IF(@ConsumerType = 1)
        BEGIN
            SELECT IsNull(PackageType, 1) AS PackageType,D.CityId       
            FROM SellInquiries S WITH (NOLOCK)
            INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = S.DealerId
            WHERE S.id = @CarId

        END
   
   
END


