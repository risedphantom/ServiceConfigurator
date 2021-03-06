﻿CREATE proc [dbo].[GetActiveRestrictions]
as
begin	
	declare @Now datetime = getdate()

	select	IR.ID
		,	TypeName
		,	[Rule]
		,	S.Hostname
		,	S.SiteName
		,	S.[Group]
		,	IR.StartMoment
		,	IR.StopMoment
		,	IR.CreateMoment
	from	IISRestriction IR
			join IISSite S on IR.IISSiteID = S.ID
	where	[Enabled] = 1
end