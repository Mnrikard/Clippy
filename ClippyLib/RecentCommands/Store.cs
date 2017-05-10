/*
 * 
 * Copyright 2012-2015 Matthew Rikard
 * This file is part of Clippy.
 * 
 *  Clippy is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  Clippy is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with Clippy.  If not, see <http://www.gnu.org/licenses/>.
 *
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using ClippyLib.Settings;

namespace ClippyLib.RecentCommands
{
    public abstract class Store
    {
		public abstract void SaveThisCommand(string editorname, string parms);
		public abstract string[] GetRecentCommandList();

		public static Store GetInstance()
		{
			SettingsObtainer obt = SettingsObtainer.CreateInstance();
			if(File.Exists(obt.RecentCommandsLocation))
			{
				return new FileStore(obt.RecentCommandsLocation);
			}
			return new RegistryStore();
		}

        
    }
}
