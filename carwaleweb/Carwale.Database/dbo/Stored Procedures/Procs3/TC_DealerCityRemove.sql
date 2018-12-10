IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DealerCityRemove]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DealerCityRemove]
GO

	-- =============================================
-- Author:		Chetan Kane
-- Create date: 4/5/2012
-- Description:	To Delete Dealer City 
-- exec [TC_DealerCityRemove] 1
-- Modified By : Tejashree Patil 
-- Added parameters BranchId and CityId 
-- Modified by Nilesh on 17-09-2013 for maintaining logs of Dealers masters for Mobile APP
-- =============================================
CREATE PROCEDURE [dbo].[TC_DealerCityRemove]
(  
 @BranchId BIGINT,
 @CityId BIGINT  
) 	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE  TC_DealerCities 
	SET IsActive = 0  
	WHERE DealerId = @BranchId AND CityId=@CityId
	
	------------------------below code is added by Nilesh on 17-09-2013 for maintaining logs of Dealers masters for Mobile APP
				
						    INSERT INTO TC_DealerMastersLog( DealerId,TableName,CreatedOn)
						    Values                         (@BranchId,'TC_DealerCities',GETDATE())
				
    ----------------------------------------------------------------------------------------------------------------------
					
	
	
	RETURN 1
END







/****** Object:  StoredProcedure [dbo].[TC_LoginFromAPI]    Script Date: 09/17/2013 19:02:36 ******/
SET ANSI_NULLS ON
