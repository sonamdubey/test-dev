IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUsedCarDealerData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUsedCarDealerData]
GO

	
-- Author		:	Sahil
-- Create date	:	03/10/2016 14:00 PM
-- Description	:	This SP will return used car dealer data for cartrade-carwale dealers.
-- =============================================   
CREATE PROCEDURE [dbo].[GetUsedCarDealerData]	

@DealerId INT,
@IsDealerMissing BIT OUTPUT,
@IsPackageMissing BIT OUTPUT,
@PackageStartDate DATETIME OUTPUT,
@PackageEndDate DATETIME OUTPUT,
@IsMigrated BIT OUTPUT

AS
BEGIN

SET NOCOUNT ON    
	SET @IsDealerMissing = 0;
    IF NOT EXISTS (Select CWDealerID from CWCTDealerMapping with(NOLOCK)
		          where CWDealerID = @DealerId)
	    BEGIN
			SET @IsDealerMissing = 1;			
			RETURN;
	    END
	
	SET @IsPackageMissing = 0;
	IF EXISTS (Select PackageId from CWCTDealerMapping with(NOLOCK)
			   where CWDealerID = @DealerId and PackageId is NULL)
		BEGIN
			SET @IsPackageMissing = 1;			
			RETURN;
		END

	Select @PackageStartDate = PackageStartDate, @PackageEndDate = PackageEndDate, @IsMigrated=IsMigrated
	from CWCTDealerMapping with(NOLOCK)
	where CWDealerID = @DealerId;

END


