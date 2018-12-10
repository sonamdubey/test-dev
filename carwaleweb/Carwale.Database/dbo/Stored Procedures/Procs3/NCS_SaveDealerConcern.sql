IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_SaveDealerConcern]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_SaveDealerConcern]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 7 May 2014
-- Description : To save dealer concern in database
-- EXEC NCS_SaveDealerConcern 2,'Test2',1
-- =============================================
CREATE PROCEDURE [dbo].[NCS_SaveDealerConcern]
	-- Add the parameters for the stored procedure here
	@OrgId NUMERIC,
	@Concern VARCHAR(2000),
	@Id BIGINT = 0 OUTPUT 
AS
BEGIN
    -- Insert statements for procedure here
	INSERT INTO NCS_DealerConcern (OrgId,Concern) VALUES(@OrgId,@Concern)
	
	SET @Id =  SCOPE_IDENTITY();
END
