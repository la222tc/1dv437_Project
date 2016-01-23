using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DashingGame.Model
{
    public class Levels
    {
         class LevelReader
        {
            public LevelReader()
            {
            }

            List<Board> _levels;
            public List<Board> Levels
            {
                get
                {
                    if ( _levels == null )
                    {
                        _levels = new List<Board>();

                        XmlDocument xd = new XmlDocument();
                        using (FileStream fs = new FileStream((string.Format("Model\\LevelInfo.xml")), FileMode.Open))
                        {
                            xd.Load(fs);
                        }

                        foreach (XmlNode levelNode in xd.SelectNodes("//levelInfo/level"))
                        {
                            _levels.Add(GetBoardFromXml(levelNode));
                        }
                    }

                    return _levels;
                }
            }

            private Board GetBoardFromXml(XmlNode levelNode)
            {
                Board board = new Board();

                board.LevelName = levelNode.Attributes["Name"].Value;

                int rowNumber = 0;
                foreach (XmlNode rowNode in levelNode.SelectNodes("row"))
                {
                    string rowData = rowNode.Attributes["data"].Value;

                    int colNumber = 0;
                    foreach (char cellData in rowData.ToCharArray())
                    {
                        switch ( cellData )
                        {
                            case '$':
                                board.AddBlock( new HeartBlock() { Y = rowNumber, X = colNumber } );
                                break;

                            case '@':
                                board.AddBlock( new BallBlock() { Y = rowNumber, X = colNumber } );
                                break;

                            case '.':
                                board.AddBlock( new SnowBlock() { Y = rowNumber, X = colNumber } );
                                break;

                            case '*':
                                board.AddBlock( new PlayerBlock() { Y = rowNumber, X = colNumber } );
                                break;

                            case '!':
                                board.AddBlock( new ExitBlock() { Y = rowNumber, X = colNumber } );
                                break;
                        
                            case 'B':
                                board.AddBlock( new BombBlock() { Y = rowNumber, X = colNumber } );
                                break;
                            case '%':
                                board.AddBlock( new WallBlock() { Y = rowNumber, X = colNumber } );
                                break;
                        }

                        colNumber++;
                    }

                    rowNumber++;
                }

                return board;
              }
        }

        LevelReader reader = new LevelReader();

        public Levels()
        {
            this.Reset();
        }

        private static Levels _instance;
        public static Levels Instance
        {
            get
            {
                if ( _instance == null )
                    _instance = new Levels();

                return _instance;
            }
        }

        public void Reset()
        {
            reader = new LevelReader();
        }

        public IEnumerable<Board> TheLevels        
        {
            get
            {
                return reader.Levels;
            }
        }
    }
}
