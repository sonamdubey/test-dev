IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_RTOSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_RTOSave]
GO

	
-- =============================================
-- ModifiedBy:		Binumon George
-- Create date: 08-11-2011
-- Description:	Added ModifiedBy parameter and modified date.
-- =============================================  
-- =============================================
-- Author:		Binumon George
-- Create date: 11th October 2011
-- Description:	This procedure is used to add update booking services
-- =============================================
CREATE PROCEDURE [dbo].[TC_RTOSave]
(
@TC_RTO_Id INT =NULL,
@DealerId NUMERIC,
@RTOName VARCHAR(50),
@ModifiedBy INT
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF(@TC_RTO_Id IS NULL) --Insering Dealer's services
	BEGIN
		IF NOT EXISTS(SELECT * FROM TC_RTO WHERE DealerId=@DealerId AND RTOName=@RTOName AND IsActive=1)
		BEGIN
			INSERT TC_RTO(RTOName,DealerId,ModifiedBy) VALUES(@RTOName,@DealerId, @ModifiedBy)
		END
		ELSE
		BEGIN
			RETURN -2 -- Means Duplicate record is already exists in DB
		END		
	END
	ELSE --  Updating  services
	BEGIN
		IF NOT EXISTS(SELECT * FROM TC_RTO WHERE DealerId=@DealerId AND TC_RTO_Id<>@TC_RTO_Id AND RTOName=@RTOName AND IsActive=1)
		BEGIN
			UPDATE TC_RTO SET RTOName=@RTOName, ModifiedBy=@ModifiedBy, ModifiedDate=GETDATE() WHERE TC_RTO_Id=@TC_RTO_Id
		END
		ELSE
		BEGIN
			RETURN -2 -- Means Duplicate record is already exists in DB
		END		
	END 	
	SELECT TC_RTO_Id ,RTOName FROM TC_RTO WHERE DealerId=@DealerId AND IsActive=1    
END



